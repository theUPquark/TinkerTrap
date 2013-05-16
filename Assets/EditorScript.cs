using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

public class EditorScript : MonoBehaviour {
	
	// Main display and storage
	
	private List<List<GameObject>> map = new List<List<GameObject>>();
	private List<List<GameObject>> mapObs = new List<List<GameObject>>();
	private string[] tileList = new string[] {"Wall","Indicator","Floor","Door","Button","Plate","Electrified", "Generator", "Source", "Finish", "Pit", "Charge"};
	private string[] obsList = new string[] {"Spawn", "Box", "Battery"};
	private string activeSelection = "empty";
	private int activeSet = 0;
	private int gridW = 10;
	private int gridH = 10;
	private string tempW = "10";
	private string tempH = "10";
	private List<int> activeCons = new List<int>();
	private List<int> activeLocks = new List<int>();
	
	// Paint/Marquee switch and selection storage
	
	private bool paintMode = true;
	private List<GameObject> boxedSelection = new List<GameObject>();
	private bool showModeList = false;
	private int modeEntry = 0;
	private GUIContent[] modeDropdown = {new GUIContent("Paint"), new GUIContent("Marquee")};
	private bool modePicked = false;
	
	// For GUI display/control
	
	private bool guiError = false;
	private bool loadFile = false;
	private bool saveFile = false;
	private bool guiInput = false;
	private FileBrowser browser;
	private string guiErS = "";
	protected string filePath;
	protected string fileName = "level";
	private double fileVersion;
	
	// Set up basic GUI
	
	public GUIStyle activeButton;
	public GUIStyle passiveButton;
	public GUISkin mainSkin;
	public Texture2D activeSelBox;		// Texture that overlays activeSelection related GUI element
	
	// For connection/lock selection dropdowns
	
	private bool showViewList = false;
	private int connectionEntry = 1;
	private int lockEntry = 1;
	private int viewEntry = 0;
	private ComboBox connectionsBox = new ComboBox();
	private ComboBox locksBox = new ComboBox();
	private GUIContent[] consDropdown = {new GUIContent("1")};
	private GUIContent[] locksDropdown = {new GUIContent("1")};
	private GUIContent[] viewDropdown = {new GUIContent("Show Active"), new GUIContent("Show All"), new GUIContent("Show Query")};
	private bool conPicked = false;
	private bool lockPicked = false;
	private bool viewPicked = false;
	
	// Tutorial Messages
	
	private int keyToAdd = 0;
	private bool showMessageList = false;
	private int bot = 0;
	private GUIContent[] messageDropdown = {new GUIContent("Bot 1"), new GUIContent("Bot 2"), new GUIContent("Bot 3")};
	private bool messPicked = false;
	
	// Drawing lines
	
	private bool showTileActives = false;
	private GameObject queryTile;
	private GameObject boxQTile;
	private bool mouse0Active = true;
	private bool validAnchor = false; // Control variable to help set tracer line
	private Vector3 anchor;
	private GameObject anchorYX;
	private List<GameObject> lineList = new List<GameObject>();
	
	private GameObject line;
	// Use this for initialization
	void Start () {
		line = GameObject.Find ("mouseLine");
		line.GetComponent<LineRenderer>().SetColors(Color.white, Color.blue);
		SetGrid();
		// Sync active lists to dropdown content
		activeCons.Add(1);
		activeLocks.Add(1);
		
		activeSelBox = Resources.Load ("Editor/ed_boxed") as Texture2D;
	}
	
	private void Sort( GUIContent[] dropList, List<int> actives) {
		actives.Sort();
		
		var intval = 	from element in dropList
		      			orderby element.text
		      			select element;
	}
	
	private bool Linkable(GameObject a, GameObject b)		//Determines if two tiles all linkable by a Conn or Lock
	{
		if (a == b) 
			return false;
		else if (a.GetComponent<EditorTile>().tileType == "Wall" || a.GetComponent<EditorTile>().tileType == "Floor" || a.GetComponent<EditorTile>().tileType == "Finish")
			return false;
		else if (b.GetComponent<EditorTile>().tileType == "Wall" || b.GetComponent<EditorTile>().tileType == "Floor" || b.GetComponent<EditorTile>().tileType == "Finish")
			return false;
		else
			return true;
	}
	
	private bool ValidSave()
	{
		bool saveMe = true;
		int countPlayerSpawn = 0;
		int countFinish = 0;
		
		foreach (List<GameObject> g in map)
		{	foreach (GameObject o in g)
			{
				if (o.GetComponent<EditorTile>().obsType == "Spawn")
					countPlayerSpawn++;
				if (o.GetComponent<EditorTile>().tileType == "Finish")
					countFinish++;
			}
		}
		if (countPlayerSpawn != 1) {
			saveMe = false;
			guiError = true;
			guiErS = "Spawn must be present and unique.\n";
		} if (countFinish != 1) {
			saveMe = false;
			guiError = true;
			guiErS += "Finish tile must be present and unique.";
		}
		return saveMe;
	}
	
	private void AddLineObject (GameObject a, GameObject b)		// Used to display a single Conn or Lock element
	{
		lineList.Add (new GameObject(lineList.Count.ToString()));
		lineList[lineList.Count - 1].AddComponent<LineRenderer>().SetVertexCount(2);
		lineList[lineList.Count - 1].GetComponent<LineRenderer>().material = new Material (Shader.Find("Particles/Additive"));
		lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetWidth (4f, 2f);
		lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetPosition(0,ReturnTileCenter(a.transform.position));
		lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetPosition(1,ReturnTileCenter(b.transform.position));
	}
	
	private void ClearLineObjects ()							// Removes all currently displayed Links
	{
		while (lineList.Count > 0)
		{
			Destroy (lineList[lineList.Count - 1]);
			lineList.RemoveAt(lineList.Count - 1);
		}
	}
	
	private Vector3 ReturnTileCenter (Vector3 v)
	{
		Vector3 change = new Vector3(v.x+16,v.y-16,v.z-10);
		return change;
	}
	
	private void DrawMouse(Vector3 a, Vector3 b)
	{
		line.GetComponent<LineRenderer>().SetWidth(6f,2f);
		line.GetComponent<LineRenderer>().SetVertexCount(2);
		line.GetComponent<LineRenderer>().SetPosition(0, a);
		line.GetComponent<LineRenderer>().SetPosition(1, b);
	}
	
	private void DragBox(Vector3 a, Vector3 c)
	{	/* a b
		 * d c
		 */
		line.GetComponent<LineRenderer>().SetColors(Color.cyan,Color.cyan);
		line.GetComponent<LineRenderer>().SetWidth(2f,2f);
		line.GetComponent<LineRenderer>().SetVertexCount(5);
		line.GetComponent<LineRenderer>().SetPosition(0,a);
		
		Vector3 b = new Vector3(c.x,a.y,a.z);
		line.GetComponent<LineRenderer>().SetPosition(1,b);
		
		line.GetComponent<LineRenderer>().SetPosition(2,c);
		
		Vector3 d = new Vector3(a.x,c.y,a.z);
		line.GetComponent<LineRenderer>().SetPosition(3,d);
		
		line.GetComponent<LineRenderer>().SetPosition(4,a);
	}
	
	private void BoxQuery() 
	{
		if (boxQTile == null) {
			boxQTile = new GameObject("Box Query");
			boxQTile.AddComponent<LineRenderer>().material = new Material (Shader.Find("Particles/Additive"));
			boxQTile.GetComponent<LineRenderer>().SetWidth (2f,2f);
			boxQTile.GetComponent<LineRenderer>().SetColors(Color.green,Color.green);
			boxQTile.GetComponent<LineRenderer>().SetVertexCount(5);
		}
		if (queryTile != null) {
			boxQTile.GetComponent<LineRenderer>().SetVertexCount(5);
			boxQTile.GetComponent<LineRenderer>().SetPosition(0,queryTile.transform.position);
			boxQTile.GetComponent<LineRenderer>().SetPosition(1,new Vector3(queryTile.transform.position.x + 32,queryTile.transform.position.y,queryTile.transform.position.z - 12));
			boxQTile.GetComponent<LineRenderer>().SetPosition(2,new Vector3(queryTile.transform.position.x + 32,queryTile.transform.position.y - 32,queryTile.transform.position.z - 12));
			boxQTile.GetComponent<LineRenderer>().SetPosition(3,new Vector3(queryTile.transform.position.x,queryTile.transform.position.y - 32,queryTile.transform.position.z - 12));;
			boxQTile.GetComponent<LineRenderer>().SetPosition(4,queryTile.transform.position);
		} else
			boxQTile.GetComponent<LineRenderer>().SetVertexCount(0);
	}
	
	private void ClearBoxedSelection()
	{
		while (boxedSelection.Count > 0)
		{
			boxedSelection.RemoveAt(boxedSelection.Count - 1);
		}
	}
	
	private void SetBoxedSelection(GameObject a, GameObject b)
	{
		ClearBoxedSelection();
		int startX = (int)a.transform.position.x/32;		// Tile values
		int startY = (int)(a.transform.position.y/-32);
		int endX = (int)b.transform.position.x/32;
		int endY = (int)b.transform.position.y/-32;
		
		bool stop = false;
		
		while (!stop)
		{
			bool nextL = false;
			int X = startX;
			while (!nextL)
			{
				boxedSelection.Add (map[startY][X]);
				if (X ==  endX)
					nextL = true;				//Last element on this line. Go next.
				else
					if (startX < endX)			//Determine which way to extend BoxedSel.
						X++;
					else 
						X--;
			}
			if (startY == endY)
				stop = true;					//Last element. End.
			else
				if (startY < endY)				//Determine which way to extend BoxedSel.
					startY++;
				else
					startY--;
		}
	}
	// Set first empty active as selection, otherwise create new active as selection
	private void CheckForEmptyActive()
	{
		int emptyActive = -1;
		
		if (activeSelection == "conn")
		{
			foreach (int act in activeCons)
			{
				bool linkFound = false;
				foreach (List<GameObject> g in map)
				{
					foreach (GameObject o in g)
					{
						if (o.GetComponent<EditorTile>().consOut.Contains(act))
						{
							linkFound = true;
						}
					}
				}
				if (!linkFound){
					emptyActive = act;
					break;
				}
			}
			if (emptyActive < 0)
			{
				activeCons.Add(activeCons.Count + 1);
				consDropdown = new GUIContent[activeCons.Count];
				for (int i = 0; i < consDropdown.Length; i++)
					consDropdown[i] = new GUIContent(activeCons[i].ToString());
				connectionEntry = activeCons[activeCons.Count - 1];
			}
			else{
				connectionEntry = activeCons[emptyActive - 1];
			}
		}
		else if (activeSelection == "lock")
		{
			foreach (int act in activeLocks)
			{
				bool linkFound = false;
				foreach (List<GameObject> g in map)
				{
					foreach (GameObject o in g)
					{
						if (o.GetComponent<EditorTile>().locksOut.Contains(act))
						{
							linkFound = true;
						}
					}
				}
				if (!linkFound){
					emptyActive = act;
					break;
				}
			}
			if (emptyActive < 0)
			{
				activeLocks.Add(activeLocks.Count + 1);
				locksDropdown = new GUIContent[activeLocks.Count];
				for (int i = 0; i < locksDropdown.Length; i++)
					locksDropdown[i] = new GUIContent(activeLocks[i].ToString());
				lockEntry = activeLocks[activeLocks.Count - 1];
			}
			else {
				lockEntry = activeLocks[emptyActive - 1];
			}
		}
	}
	
	private void DrawLinks()
	{
		ClearLineObjects();
		if (viewEntry == 0) // "Show Active"
		{
			if (activeSelection == "conn")
				DrawConnections(connectionEntry);
			else if (activeSelection == "lock")
				DrawLocks(lockEntry);
		}
		else if (viewEntry == 1) // "Show All"
		{
			foreach (int c in activeCons)
				DrawConnections(c);
			foreach (int l in activeLocks)
				DrawLocks(l);
		}
		else if (viewEntry == 2 && queryTile != null)
		{
			List<int> queryC = new List<int>();
			List<int> queryL = new List<int>();
			foreach (int cI in queryTile.GetComponent<EditorTile>().consIn)
				queryC.Add (cI);
			foreach (int cO in queryTile.GetComponent<EditorTile>().consOut)
				if (!queryC.Contains(cO))
					queryC.Add (cO);
			foreach (int lI in queryTile.GetComponent<EditorTile>().locksIn)
				queryL.Add (lI);
			foreach (int lO in queryTile.GetComponent<EditorTile>().locksOut)
				if (!queryL.Contains(lO))
					queryL.Add (lO);
			
			foreach (int c in queryC)
				DrawConnections(c);
			foreach (int l in queryL)
				DrawLocks(l);
		}
	}
	
	private void CheckAllLinks() {
		if (viewEntry != 1) {
			viewEntry = 1;
			DrawLinks();
		} else
			DrawLinks();	
	}
	
	private void DrawConnections(int num)
	{
		bool outFound = false;
		foreach (List<GameObject> g in map)
		{
			foreach (GameObject o in g)
			{
				// Checking all tiles for num, using consOut
				if ( o.GetComponent<EditorTile>().consOut.Contains(num))
				{
					outFound = true;
					bool inFound = false;
					foreach (List<GameObject> g2 in map)
					{
						foreach (GameObject o2 in g2)
						{
							// Check all tiles again for num, using consIn
							if (o2.GetComponent<EditorTile>().consIn.Contains(num))
							{
								inFound = true;
								AddLineObject(o,o2);
								if (num == connectionEntry) // Visual distinction for modifiable entry
								{
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetColors(new Color(197f/255f,244f/255f,184f/255f),Color.green);
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetWidth(6f, 2f);
								}
								else
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetColors (new Color(197f/255f,244f/255f,184f/255f), new Color(22f/255f,148f/255f,64f/255f));
							}
						}
					}
					if (!inFound) {
						o.GetComponent<EditorTile>().consOut.Remove(num); // Removes lone Outs
					}
				}
			}
		}
		if (!outFound) { // If no Outs found, check for and remove any Ins
			foreach (List<GameObject> g in map)
			{
			foreach (GameObject o in g)
				{
					if (o.GetComponent<EditorTile>().consIn.Contains(num))
						o.GetComponent<EditorTile>().consIn.Remove (num);
				}
			}
		}
	}
	
	private void DrawLocks(int num)
	{
		bool outFound = false;
		foreach (List<GameObject> g in map)
		{
			foreach (GameObject o in g)
			{
				// Checking all tiles for num, using locksOut
				if ( o.GetComponent<EditorTile>().locksOut.Contains(num))
				{
					outFound = true;
					bool inFound = false;
					foreach (List<GameObject> g2 in map)
					{
						foreach (GameObject o2 in g2)
						{
							// Check all tiles again for num, using locksin
							if (o2.GetComponent<EditorTile>().locksIn.Contains(num))
							{
								inFound = true;
								AddLineObject(o,o2);
								if (num == lockEntry) // Visual distinction for modifiable entry
								{
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetColors(new Color(197f/255f,244f/255f,184f/255f),new Color(206f/255f,58f/255f,32f/255f,255f/255f));
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetWidth(6f, 2f);
								}
								else
									lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetColors (new Color(236f/255f,243f/255f,183f/255f,255f/255f), new Color(106f/255f,58f/255f,32f/255f,255f/255f));
							}
						}
					}
					if (!inFound) {
						o.GetComponent<EditorTile>().locksOut.Remove(num); // Removes lone Outs
					}
				}
			}
		}
		if (!outFound) { // If no Outs found, check for and remove any Ins
			foreach (List<GameObject> g in map)
			{
			foreach (GameObject o in g)
				{
					if (o.GetComponent<EditorTile>().locksIn.Contains(num))
						o.GetComponent<EditorTile>().locksIn.Remove (num);
				}
			}
		}
	}
	
	private string printList (List<int> coll) {
		string makeString = "";
		if (coll.Count > 0)
			makeString = coll[0].ToString();
		for (int count = 1; count < coll.Count; count++)
			makeString += ", " + coll[count].ToString();
		return makeString;
	}
	
	private void SetGrid() {
		bool downSized = false;
		for (int i = 0; i < gridH; i++) {
			if (map.Count == i) {
				map.Add (new List<GameObject>());
				mapObs.Add (new List<GameObject>());
			}
			for (int j = 0; j < gridW; j++) {
				if (map[i].Count == j) {
					map[i].Add(OT.CreateObject ("builderSprite"));
					mapObs[i].Add(OT.CreateObject ("builderSprite"));
					map[i][j].AddComponent<EditorTile>();
					map[i][j].GetComponent<OTSprite>().position = new Vector2(j*32f,i*-32f);
					mapObs[i][j].GetComponent<OTSprite>().position = new Vector2(j*32f,i*-32f);
					map[i][j].GetComponent<OTSprite>().frameName = "ed_Wall0";
					mapObs[i][j].GetComponent<OTSprite>().frameName = "empty";
					mapObs[i][j].GetComponent<OTSprite>().depth = -1;
				}
			}
			if (gridW < map[i].Count) {
				downSized = true;
				foreach (GameObject t in map[i].GetRange (gridW,map[i].Count-gridW))
					Destroy (t);
				foreach (GameObject o in mapObs[i].GetRange (gridW,map[i].Count-gridW))
					Destroy (o);
				map[i].RemoveRange (gridW,map[i].Count-gridW);
				mapObs[i].RemoveRange (gridW,mapObs[i].Count-gridW);
			}
		}
		if (gridH < map.Count) {
			downSized = true;
			foreach (List<GameObject> l in map.GetRange (gridH,map.Count-gridH))
				foreach (GameObject t in l)
					Destroy (t);
			foreach (List<GameObject> l in mapObs.GetRange (gridH,map.Count-gridH))
				foreach (GameObject o in l)
					Destroy (o);
			map.RemoveRange (gridH,map.Count-gridH);
			mapObs.RemoveRange (gridH,map.Count-gridH);
		}
		// Redraw links on downsize to clear any resulting broken links.
		if (downSized == true) 
			CheckAllLinks();
	}
	
	// Update is called once per frame
	void Update() {
		if (!guiError && !loadFile && !saveFile && !guiInput){
			if (Input.GetKeyDown(KeyCode.LeftAlt)) {
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				int selectX = (int)(Math.Floor (mouseLocation.x/32));
				int selectY = (int)(Math.Floor (mouseLocation.y/-32));
				if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
					if (queryTile == map[selectY][selectX]) {
						if (!showTileActives)  {
							showTileActives = true;
						} else
							showTileActives = false;
					} else {
						queryTile = map[selectY][selectX];
						BoxQuery();
						showTileActives = true;
						if (viewEntry == 2)
							DrawLinks();
					}
				} else {
					showTileActives = false;
					queryTile = null;
					BoxQuery();
					if (viewEntry == 2)
						DrawLinks();
				}
			}
			if (!paintMode && activeSelection != "conn" && activeSelection != "lock") {
				if (Input.GetMouseButtonDown (0)) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						validAnchor = true;
						anchorYX = map[selectY][selectX];
						anchor = ReturnTileCenter(map[selectY][selectX].transform.position);
					}
				} 
				else if (Input.GetMouseButtonUp (0) && validAnchor == true) {
					validAnchor = false;
				}
				if (Input.GetMouseButton (0) && validAnchor == true) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						SetBoxedSelection(anchorYX,map[selectY][selectX]);
						Vector3 pointB = new Vector3(map[selectY][selectX].transform.position.x,map[selectY][selectX].transform.position.y,map[selectY][selectX].transform.position.z);
						if (anchor.x < map[selectY][selectX].transform.position.x)
							if (anchor.y > map[selectY][selectX].transform.position.y)
								DragBox(new Vector3(anchor.x - 16,anchor.y + 16, anchor.z),new Vector3(pointB.x + 32, pointB.y - 32, pointB.z));
							else
								DragBox(new Vector3(anchor.x - 16,anchor.y - 16, anchor.z),new Vector3(pointB.x + 32, pointB.y, pointB.z));
						else
							if (anchor.y > map[selectY][selectX].transform.position.y)
								DragBox(new Vector3(anchor.x + 16,anchor.y + 16, anchor.z),new Vector3(pointB.x, pointB.y - 32, pointB.z));
							else
								DragBox(new Vector3(anchor.x + 16,anchor.y - 16, anchor.z),new Vector3(pointB.x, pointB.y, pointB.z));
					}
				}
				
			} else if (tileList.Contains (activeSelection)) {
				if (Input.GetMouseButton (0)) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						SetTypeByDraw(map[selectY][selectX]);
						SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
					}
				} else if (Input.GetMouseButton (1)) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						map[selectY][selectX].GetComponent<EditorTile>().ClearConstraints();
						SetTypeByDraw(map[selectY][selectX]);
						SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
						CheckAllLinks();
					}
				}
			} else if (obsList.Contains (activeSelection)) {
				if (Input.GetMouseButton (0)) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						SetObsByDraw(map[selectY][selectX]);
						SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
					}
				} else if (Input.GetMouseButton (1)) {
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						map[selectY][selectX].GetComponent<EditorTile>().ClearConstraints();
						SetObsByDraw(map[selectY][selectX]);
						SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
						CheckAllLinks();
					}
				}
			} else if (activeSelection == "empty" && Input.GetMouseButton (0)) {
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				int selectX = (int)(Math.Floor (mouseLocation.x/32));
				int selectY = (int)(Math.Floor (mouseLocation.y/-32));
				if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
					map[selectY][selectX].GetComponent<EditorTile>().obsType = "";
					SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
				}
			} else if (Input.GetMouseButtonDown (0)) {
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				int selectX = (int)(Math.Floor (mouseLocation.x/32));
				int selectY = (int)(Math.Floor (mouseLocation.y/-32));
				if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
					mouse0Active = true;
					line.GetComponent<LineRenderer>().SetColors(Color.white, Color.blue);
					validAnchor = true;
					anchorYX = map[selectY][selectX];
					anchor = ReturnTileCenter(map[selectY][selectX].transform.position);
				}
			} else if (activeSelection == "conn" || activeSelection == "lock"){
				if (Input.GetMouseButtonDown (1))
				{
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						mouse0Active = false;
						line.GetComponent<LineRenderer>().SetColors(Color.white, Color.red);
						validAnchor = true;
						anchorYX = map[selectY][selectX];
						anchor = ReturnTileCenter(map[selectY][selectX].transform.position);
					}
				}
				if ((Input.GetMouseButton (0) || Input.GetMouseButton(1)) && validAnchor == true)
				{
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					DrawMouse(anchor,mouseLocation);
				}
				if (mouse0Active && Input.GetMouseButtonUp (0))
				{
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					validAnchor = false;
					if (Input.GetKey(KeyCode.LeftControl)){ // Holding control creates new link in a new group
						if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
							if (activeSelection == "conn" && Linkable(map[selectY][selectX], anchorYX)){
								CheckForEmptyActive();
								// Set link
								map[selectY][selectX].GetComponent<EditorTile>().consIn.Add(connectionEntry);
								anchorYX.GetComponent<EditorTile>().consOut.Add(connectionEntry);
								DrawLinks();
							}
							else if (activeSelection == "lock" && Linkable(map[selectY][selectX], anchorYX)) {
								CheckForEmptyActive();
								//Set link
								map[selectY][selectX].GetComponent<EditorTile>().locksIn.Add(lockEntry);
								anchorYX.GetComponent<EditorTile>().locksOut.Add (lockEntry);
								DrawLinks();
							}
						}
					}
					else {
						if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
							// Set the connection (20) or lockgroup (21)
							if (activeSelection == "conn" && Linkable(map[selectY][selectX], anchorYX)) 
							{	if (!map[selectY][selectX].GetComponent<EditorTile>().consIn.Contains(connectionEntry)&& !map[selectY][selectX].GetComponent<EditorTile>().consOut.Contains(connectionEntry)) //Don't add node if one already exists
									map[selectY][selectX].GetComponent<EditorTile>().consIn.Add(connectionEntry);
								if (!anchorYX.GetComponent<EditorTile>().consOut.Contains(connectionEntry) && !anchorYX.GetComponent<EditorTile>().consIn.Contains(connectionEntry))
									anchorYX.GetComponent<EditorTile>().consOut.Add(connectionEntry);
								DrawLinks();
							} 
							else if (activeSelection == "lock" && Linkable(map[selectY][selectX], anchorYX))
							{	if (!map[selectY][selectX].GetComponent<EditorTile>().locksIn.Contains(lockEntry) && !map[selectY][selectX].GetComponent<EditorTile>().locksOut.Contains(lockEntry))
									map[selectY][selectX].GetComponent<EditorTile>().locksIn.Add(lockEntry);
								if (!anchorYX.GetComponent<EditorTile>().locksOut.Contains(lockEntry) && !anchorYX.GetComponent<EditorTile>().locksIn.Contains(lockEntry))
									anchorYX.GetComponent<EditorTile>().locksOut.Add (lockEntry);
								DrawLinks();
							}
						}
					}
					DrawMouse(anchor,anchor); // Removing mouse line from view
				}
				else if (!mouse0Active && Input.GetMouseButtonUp (1)) 
				{
					Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
					int selectX = (int)(Math.Floor (mouseLocation.x/32));
					int selectY = (int)(Math.Floor (mouseLocation.y/-32));
					validAnchor = false;
					if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
						// Remove connection or lockgroup
						if (activeSelection == "conn") {
							if (map[selectY][selectX].GetComponent<EditorTile>().consIn.Contains(connectionEntry))
								map[selectY][selectX].GetComponent<EditorTile>().consIn.Remove(connectionEntry);
							if (anchorYX.GetComponent<EditorTile>().consOut.Contains(connectionEntry))
								anchorYX.GetComponent<EditorTile>().consOut.Remove(connectionEntry);
							DrawLinks();
						} else if (activeSelection == "lock") {
							if (map[selectY][selectX].GetComponent<EditorTile>().locksIn.Contains(lockEntry))
								map[selectY][selectX].GetComponent<EditorTile>().locksIn.Remove(lockEntry);
							if (anchorYX.GetComponent<EditorTile>().locksOut.Contains(lockEntry))
								anchorYX.GetComponent<EditorTile>().locksOut.Remove(lockEntry);
							DrawLinks();
						}							
					}
					DrawMouse(anchor,anchor);
				}
			}
		}
	}
	
	void LateUpdate () {
			transform.position = new Vector3(camera.orthographicSize*((float)(Screen.width)/(float)(Screen.height))-20, -camera.orthographicSize*(1-150f/Screen.height), transform.position.z);
	}
	
	private void SetTypeByDraw(GameObject a)
	{
		//Stop the case where an obstacle is present and the pending tile type is invalid (wall/door/finish)
		if(!(!(a.GetComponent<EditorTile>().obsType == "") && (activeSelection == "Wall" || activeSelection == "Door" || activeSelection == "Finish" ||
																activeSelection == "Button" || activeSelection == "Generator" /*|| activeSelection == "Source"*/)))
		{	// Stop Wall or Floor values if any in/out Link is present
			if (!((activeSelection == "Wall" || activeSelection == "Floor")  && (a.GetComponent<EditorTile>().consIn.Count != 0 || a.GetComponent<EditorTile>().consOut.Count != 0 
				|| a.GetComponent<EditorTile>().locksIn.Count != 0 || a.GetComponent<EditorTile>().locksOut.Count != 0))) {
				a.GetComponent<EditorTile>().tileType = activeSelection;
				a.GetComponent<EditorTile>().tileSet = activeSet;
			}
		}
	}
	
	private void SetObsByDraw(GameObject a)
	{
		//Stop the case where the tile type is invalid (wall/door/finish)
		if (!(a.GetComponent<EditorTile>().tileType == "Wall" || a.GetComponent<EditorTile>().tileType == "Door" || a.GetComponent<EditorTile>().tileType == "Finish" ||
				a.GetComponent<EditorTile>().tileType == "Button" || a.GetComponent<EditorTile>().tileType == "Generator" /*|| a.GetComponent<EditorTile>().tileType == "Source"*/))
		{
			a.GetComponent<EditorTile>().obsType = activeSelection;
		}
	}
	
	private void SetGraphics(GameObject a, GameObject b)
	{
		a.GetComponent<OTSprite>().frameName =
			"ed_"+a.GetComponent<EditorTile>().tileType+
			a.GetComponent<EditorTile>().tileSet.ToString ();
		
		if (a.GetComponent<EditorTile>().obsType == "")
			b.GetComponent<OTSprite>().frameName = "empty";
		else {
			b.GetComponent<OTSprite>().frameName =
				"ed_"+a.GetComponent<EditorTile>().obsType+"0";
		}
	}
	
	private FileBrowser BrowserSetup()
	{
		FileBrowser browser;
		if (loadFile) {
			browser = new FileBrowser(
				new Rect(Screen.width/2-300, 100, 600, 300),
				"Choose XML File",
				ReturnLoadPath
			);
			browser.SelectionPattern = "*.xml";
			browser.BrowserType = FileBrowserType.File;
		} else {
			browser = new FileBrowser(
				new Rect(Screen.width/2-300, 200, 600, 300),
				"Choose Location and Name",
				ReturnSavePath
			);
			browser.BrowserType = FileBrowserType.Directory;
		}
		return browser;
	}
	
	private void ReturnLoadPath(string path)
	{
		if (filePath == null)
			loadFile = false;
		filePath = path;
	}
	
	private void ReturnSavePath(string path)
	{
		if (filePath == null)
			saveFile = false;
		filePath = path + "/" + fileName + ".xml";
	}
	
	private void WriteXML()
    {
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		using (XmlWriter writer = XmlWriter.Create(filePath,settings)) {
			writer.WriteStartDocument ();
			writer.WriteStartElement ("document");
			writer.WriteElementString ("version", "3.0");
			writer.WriteElementString ("width", gridW.ToString ());
			writer.WriteElementString ("height", gridH.ToString ());
			foreach (List<GameObject> i in map) {
				writer.WriteStartElement ("y");
				foreach (GameObject j in i) {
					writer.WriteStartElement ("x");
					writer.WriteElementString ("type",j.GetComponent<EditorTile>().tileType);
					writer.WriteElementString ("tset",j.GetComponent<EditorTile>().tileSet.ToString ());
					writer.WriteElementString ("obs",j.GetComponent<EditorTile>().obsType);
					writer.WriteStartElement ("connections");
					foreach (int cons in j.GetComponent<EditorTile>().consIn) {
						writer.WriteElementString ("in", cons.ToString());
					}
					foreach (int cons in j.GetComponent<EditorTile>().consOut) {
						writer.WriteElementString ("out", cons.ToString());
					}
					writer.WriteEndElement ();
					writer.WriteStartElement ("locks");
					foreach (int locks in j.GetComponent<EditorTile>().locksIn) {
						writer.WriteElementString ("in", locks.ToString());
					}
					foreach (int locks in j.GetComponent<EditorTile>().locksOut) {
						writer.WriteElementString ("out", locks.ToString());
					}
					writer.WriteEndElement ();
					writer.WriteStartElement ("tutorial");
					int botNum = 0;
					foreach (var bot in j.GetComponent<EditorTile>().botMessage) {
						botNum++;
						writer.WriteStartElement ("bot"+botNum.ToString());
						foreach (KeyValuePair<int, string> msg in j.GetComponent<EditorTile>().botMessage[botNum-1]) {
							writer.WriteElementString ("level", msg.Key.ToString());
							writer.WriteElementString ("msg",msg.Value);
						}
						writer.WriteEndElement ();
					}
					writer.WriteEndElement ();
					writer.WriteEndElement ();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndDocument ();
		}
		filePath = null;
    }
	
	private void LoadLevel()
	{
		XmlReaderSettings settings = new XmlReaderSettings();
		settings.IgnoreWhitespace = true;
		activeCons.Clear ();
		activeLocks.Clear ();
		foreach (List<GameObject> t in map) {
			foreach (GameObject i in t) {
				i.GetComponent<EditorTile>().consIn.Clear ();
				i.GetComponent<EditorTile>().consOut.Clear ();
				i.GetComponent<EditorTile>().locksIn.Clear ();
				i.GetComponent<EditorTile>().locksOut.Clear ();
			}
		}
		using (XmlReader read = XmlReader.Create (filePath, settings)) {
			int i = -1;
			int j = -1;
			bool consGroup = true;
			int botGroup = -1;
			int lvl = 0;
			while (read.Read ()) {
				if (read.IsStartElement ())
				{
					switch (read.Name)
					{
					case "version":
						read.Read ();
						fileVersion = read.ReadContentAsDouble ();
						break;
					case "width":
						if (fileVersion <= 2.0) {
							read.Close ();
							LoadOldLevel ();
							break;
						}
						read.Read ();
						tempW = read.Value;
						gridW = int.Parse (tempW);
						break;
					case "height":
						read.Read ();
						tempH = read.Value;
						gridH = int.Parse (tempH);
						SetGrid();
						break;
					case "y":
						if (j >= 0)
							SetGraphics(map[j][i],mapObs[j][i]);
						j++;
						i = -1;
						Console.WriteLine (j.ToString());
						break;
					case "x":
						if (i >= 0 || (j == gridH-1 && i == gridW-1))
							SetGraphics(map[j][i],mapObs[j][i]);
						else
							SetGraphics(map[j][gridW-1],mapObs[j][gridW-1]);
						Console.WriteLine (i.ToString());
						i++;
						break;
					case "type":
						read.Read ();
						map[j][i].GetComponent<EditorTile>().tileType = read.Value;
						break;
					case "tset":
						read.Read ();
						map[j][i].GetComponent<EditorTile>().tileSet = int.Parse (read.Value);
						break;
					case "obs":
						if (!read.IsEmptyElement) {
							read.Read ();
							map[j][i].GetComponent<EditorTile>().obsType = read.Value;
						}
						break;
					case "connections":
						Debug.Log ("This is a connection...");
						consGroup = true;
						break;
					case "locks":
						Debug.Log ("This is a lock...");
						consGroup = false;
						break;
					case "in":
						read.Read ();
						int node = read.ReadContentAsInt();
						if (consGroup) {
							Debug.Log ("Writing in con...");
							map[j][i].GetComponent<EditorTile>().consIn.Add(node);
							if (!activeCons.Contains (node))
								activeCons.Add (node);
						} else {
							Debug.Log ("Writing in lock...");
							map[j][i].GetComponent<EditorTile>().locksIn.Add(node);
							if (!activeLocks.Contains (node))
								activeLocks.Add (node);
						}
						break;
					case "out":
						read.Read ();
						node = read.ReadContentAsInt();
						if (consGroup) {
							Debug.Log ("Writing out con...");
							map[j][i].GetComponent<EditorTile>().consOut.Add(node);
							if (!activeCons.Contains (node))
								activeCons.Add (node);
						} else {
							Debug.Log ("Writing out lock...");
							map[j][i].GetComponent<EditorTile>().locksOut.Add(node);
							if (!activeLocks.Contains (node))
								activeLocks.Add (node);
						}
						break;
					case "tutorial":
						break;
					case "bot1":
						botGroup = 0;
						break;
					case "bot2":
						botGroup = 1;
						break;
					case "bot3":
						botGroup = 2;
						break;
					case "level":
						read.Read();
						lvl = int.Parse (read.Value);
						break;
					case "msg":
						read.Read ();
						string msg = read.Value;
						map[j][i].GetComponent<EditorTile>().botMessage[botGroup].Add (lvl,msg);
						break;
					}
				}
			}
			read.Close ();
			filePath = null;
		}
		if (activeCons.Count == 0)
			activeCons.Add (1);
		if (activeLocks.Count == 0)
			activeLocks.Add (1);
		Sort (consDropdown,activeCons);
		Sort (locksDropdown,activeLocks);
	}
	
	// Old level loader for conversion purposes
	
	private void LoadOldLevel()
	{
		XmlReaderSettings settings = new XmlReaderSettings();
		settings.IgnoreWhitespace = true;
		using (XmlReader read = XmlReader.Create (filePath, settings)) {
			int i = -1;
			int j = -1;
			bool consGroup = true;
			while (read.Read ()) {
				if (read.IsStartElement ())
				{
					switch (read.Name)
					{
					case "width":
						read.Read ();
						tempW = read.Value;
						gridW = int.Parse (tempW);
						break;
					case "height":
						read.Read ();
						tempH = read.Value;
						gridH = int.Parse (tempH);
						SetGrid();
						break;
					case "y":
						if (j >= 0)
							SetGraphics(map[j][i],mapObs[j][i]);
						j++;
						i = -1;
						Console.WriteLine (j.ToString());
						break;
					case "x":
						if (i >= 0 || (j == gridH-1 && i == gridW-1))
							SetGraphics(map[j][i],mapObs[j][i]);
						else
							SetGraphics(map[j][gridW-1],mapObs[j][gridW-1]);
						Console.WriteLine (i.ToString());
						i++;
						break;
					case "type":
						read.Read ();
						int type = int.Parse (read.Value);
						string tstr = "Wall";
						int tset = 0;
						switch (type) {
						case 0:
							tstr = "Floor";
							break;
						case 2:
							tstr = "Plate";
							break;
						case 3:
						case 6:
							tstr = "Button";
							break;
						case 4:
							tstr = "Door";
							tset = 1;
							break;
						case 5:
							tstr = "Door";
							break;
						}
						map[j][i].GetComponent<EditorTile>().tileType = tstr;
						map[j][i].GetComponent<EditorTile>().tileSet = tset;
						break;
					case "obs":
						read.Read ();
						type = int.Parse (read.Value);
						tstr = "";
						tset = 0;
						switch (type) {
						case 1:
							tstr = "Spawn";
							break;
						case 4:
							tstr = "Box";
							break;
						}
						map[j][i].GetComponent<EditorTile>().obsType = tstr;
						break;
					case "connections":	
						consGroup = true;
						break;
					case "locks":
						consGroup = false;
						break;
					case "int":
						read.Read ();
						int node = read.ReadContentAsInt();
						if (consGroup) {
							map[j][i].GetComponent<EditorTile>().consIn.Add(node);
							map[j][i].GetComponent<EditorTile>().consOut.Add(node);
							if (!activeCons.Contains (node))
								activeCons.Add (node);
						} else {
							map[j][i].GetComponent<EditorTile>().locksIn.Add(node);
							map[j][i].GetComponent<EditorTile>().locksOut.Add(node);
							if (!activeLocks.Contains (node))
								activeLocks.Add (node);
						}
						break;
					case "in":
						read.Read ();
						node = read.ReadContentAsInt();
						if (consGroup) {
							map[j][i].GetComponent<EditorTile>().consIn.Add(node);
							if (!activeCons.Contains (node))
								activeCons.Add (node);
						} else {
							map[j][i].GetComponent<EditorTile>().locksIn.Add(node);
							if (!activeLocks.Contains (node))
								activeLocks.Add (node);
						}
						break;
					case "out":
						read.Read ();
						node = read.ReadContentAsInt();
						if (consGroup) {
							map[j][i].GetComponent<EditorTile>().consOut.Add(node);
							if (!activeCons.Contains (node))
								activeCons.Add (node);
						} else {
							map[j][i].GetComponent<EditorTile>().locksOut.Add(node);
							if (!activeLocks.Contains (node))
								activeLocks.Add (node);
						}
						break;
					}
				}
			}
			read.Close ();
			filePath = null;
		}
	}
	
	void OnGUI () {
		guiInput = false;
		GUI.skin = mainSkin;
		GUI.BeginGroup (new Rect(0,0,Screen.width,Screen.height));
		GUI.Label (new Rect(5,5,60,30), "Width:");
		GUI.Label (new Rect(70,5,60,30), "Height:");
		tempW = GUI.TextField (new Rect(5,35,30,30), tempW, 2);
		tempH = GUI.TextField (new Rect(70,35,30,30), tempH, 2);
		if (GUI.Button (new Rect(135,35,60,30), "Apply")) {
			guiInput = true;
			int intTempH = 0;
			int intTempW = 0;
			try {
				intTempW = int.Parse (tempW);
				intTempH = int.Parse (tempH);
				if (intTempW*intTempH <= 0) {
					tempW = gridW.ToString ();
					tempH = gridH.ToString ();
					guiError = true;
					guiErS = "Greater than zero, please.";
				} else {
					gridW = intTempW;
					gridH = intTempH;
					SetGrid();
				}
			} catch (FormatException b) {
				tempW = gridW.ToString ();
				tempH = gridH.ToString ();
				guiError = true;
				guiErS = "Only whole numbers, please.";
			}
		}
		
		if (guiError) {
			GUI.Label (new Rect (Screen.width/2 - 150, 70, 300, 100), guiErS, "box");
			if (GUI.Button (new Rect(Screen.width/2 - 30, 135, 60, 30), "Okay.")) {
				guiInput = true;
				guiError = false;
				guiErS = "";
			}
		}
		
		if (Popup.List (new Rect(Screen.width - 149,5,70,30), ref showModeList, ref modeEntry, modeDropdown[modeEntry], modeDropdown, activeButton)) {
			modePicked = true;
			activeSelection = "";
			if (modeEntry == 0) {
				paintMode = true;
				line.GetComponent<LineRenderer>().SetVertexCount(0);
				ClearBoxedSelection();
			} else {
				paintMode = false;
			}
		} else
			modePicked = false;
		
		GUI.Label (new Rect(Screen.width - 78,5,72,(tileList.Length/2+1+tileList.Length%2)*37), "Tiles", "box");
		for (int i = 0; i < tileList.Length; i++) {
			GUIStyle buttonStyle;
			if (tileList[i] == activeSelection)
				buttonStyle = activeButton;
			else
				buttonStyle = passiveButton;
			Texture tex;
			int tSet = 0;
			if (activeSelection.Equals(tileList[i])) {
				if (Resources.Load ("Editor/ed_"+tileList[i]+(activeSet+1).ToString ()) == null) {
					tex = (Texture) Resources.Load ("Editor/ed_"+tileList[i]+0.ToString (), typeof(Texture));
				} else {
					tex = (Texture) Resources.Load ("Editor/ed_"+tileList[i]+(activeSet+1).ToString (), typeof(Texture));
					tSet = activeSet+1;
				}
			} else
				tex = (Texture) Resources.Load ("Editor/ed_"+tileList[i]+0.ToString (), typeof(Texture));
			if (GUI.Button (new Rect(Screen.width-(32*(2-i%2))-10,(32+5)*(i/2)+40,32,32), tex, buttonStyle)) {
				guiInput = true;
				activeSelection = tileList[i];
				activeSet = tSet;
				if (!paintMode && boxedSelection.Count > 0){
					if (Event.current.button == 0) {
						foreach (GameObject o in boxedSelection){
							SetTypeByDraw(o);
							SetGraphics(o,mapObs[(int)o.transform.position.y/-32][(int)o.transform.position.x/32]);
						}
					} else if (Event.current.button == 1) {
						foreach (GameObject o in boxedSelection){
							o.GetComponent<EditorTile>().ClearConstraints();
							SetTypeByDraw(o);
							SetGraphics(o,mapObs[(int)o.transform.position.y/-32][(int)o.transform.position.x/32]);
						}
						CheckAllLinks();
					}
				}
			}
			if (paintMode && tileList[i] == activeSelection) {
				GUI.DrawTexture (new Rect(Screen.width-(32*(2-i%2))-10,(32+5)*(i/2)+40,32,32),activeSelBox); }
		}
		
		GUI.Label (new Rect(Screen.width - 78,(32+5)*(tileList.Length/2+1)+40,72,120), "Spawns", "box");
		for (int i = 0; i < obsList.Length+1; i++) {
			Texture tex;
			int oSet = 0;
			GUIStyle buttonStyle;
			if (i == obsList.Length) {
				buttonStyle = passiveButton;
				tex = (Texture) Resources.Load ("Editor/empty", typeof(Texture));
				if (GUI.Button (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length-(tileList.Length%2)+i+4)/2)+40,32,32), tex, buttonStyle)) {
					guiInput = true;
					activeSelection = "empty";
					if (!paintMode && boxedSelection.Count > 0){
						foreach (GameObject o in boxedSelection){
							o.GetComponent<EditorTile>().obsType = "";
							SetGraphics(o,mapObs[(int)o.transform.position.y/-32][(int)o.transform.position.x/32]);
						}
					}
				}
				if (paintMode && activeSelection == "empty") {
					GUI.DrawTexture (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length-(tileList.Length%2)+i+4)/2)+40,32,32),activeSelBox); }
			} else {
				if (obsList[i].Equals (activeSelection))
					buttonStyle = activeButton;
				else
					buttonStyle = passiveButton;
				if (activeSelection.Equals(obsList[i])) {
					if (Resources.Load ("Editor/ed_"+obsList[i]+(activeSet+1).ToString ()) == null)
						tex = (Texture) Resources.Load ("Editor/ed_"+obsList[i]+activeSet.ToString (), typeof(Texture));
					else
						tex = (Texture) Resources.Load ("Editor/ed_"+obsList[i]+(activeSet+1).ToString (), typeof(Texture));
				} else
					tex = (Texture) Resources.Load ("Editor/ed_"+obsList[i]+0.ToString (), typeof(Texture));
				if (GUI.Button (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length-(tileList.Length%2)+i+4)/2)+40,32,32), tex, buttonStyle)) {
					guiInput = true;
					activeSelection = obsList[i];
					activeSet = oSet;
					if (!paintMode && boxedSelection.Count > 0){
						if (Event.current.button == 0) {
							foreach (GameObject o in boxedSelection){
								SetObsByDraw(o);
								SetGraphics(o,mapObs[(int)o.transform.position.y/-32][(int)o.transform.position.x/32]);
							}
						} else if (Event.current.button == 1) {
							foreach (GameObject o in boxedSelection){
								o.GetComponent<EditorTile>().ClearConstraints();
								SetObsByDraw(o);
								SetGraphics(o,mapObs[(int)o.transform.position.y/-32][(int)o.transform.position.x/32]);
							}
							CheckAllLinks();
						}
					}
				}
				if (paintMode && obsList[i] == activeSelection) {
					GUI.DrawTexture (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length-(tileList.Length%2)+i+4)/2)+40,32,32),activeSelBox); }
			}
		}
		
		if (GUI.Button (new Rect(200,5,90,60), "Save") && (!saveFile && !loadFile) ) { // Changed '||' to '&&' so filebrower won't open both load and save windows
			if (ValidSave()) {
				guiInput = true;
				saveFile = true;
				browser = BrowserSetup ();
			}
		}
		
		if (GUI.Button (new Rect(295,5,90,60), "Load") && (!saveFile && !loadFile) ) { // Changed '||' to '&&' so filebrower won't open both load and save windows
			guiInput = true;
			loadFile = true;
			browser = BrowserSetup ();
		}
		
		if (Popup.List (new Rect(Screen.width - 124,(32+5)*(tileList.Length/2+obsList.Length/2)+170,100,25), ref showViewList, ref viewEntry, viewDropdown[viewEntry], viewDropdown, activeButton)) {
			viewPicked = true;
			DrawLinks();
		} else
			viewPicked = false;
		
		GUI.Label (new Rect(Screen.width - 147,(32+5)*(tileList.Length/2+obsList.Length/2)+208,141,75), "Connections", "box");
		if (activeSelection == "conn") {
			GUI.DrawTexture (new Rect(Screen.width - 147,(32+5)*(tileList.Length/2+obsList.Length/2)+208,141,75),activeSelBox);
		}
		if (showTileActives){
			if (queryTile.GetComponent<EditorTile>().consIn.Count > 0)
				GUI.Label (new Rect(Screen.width - 187 - 5*printList(queryTile.GetComponent<EditorTile>().consIn).Length,(32+5)*(tileList.Length/2+obsList.Length/2)+208,40 + 5*printList(queryTile.GetComponent<EditorTile>().consIn).Length,30),"In: " + printList(queryTile.GetComponent<EditorTile>().consIn), "box");
			if (queryTile.GetComponent<EditorTile>().consOut.Count > 0)
				GUI.Label (new Rect(Screen.width - 197 - 5*printList(queryTile.GetComponent<EditorTile>().consOut).Length,(32+5)*(tileList.Length/2+obsList.Length/2)+248,50 + 5*printList(queryTile.GetComponent<EditorTile>().consOut).Length,30),"Out: " + printList(queryTile.GetComponent<EditorTile>().consOut), "box");
			if (queryTile.GetComponent<EditorTile>().locksIn.Count > 0)
				GUI.Label (new Rect(Screen.width - 187 - 5*printList(queryTile.GetComponent<EditorTile>().locksIn).Length,(32+5)*(tileList.Length/2+obsList.Length/2)+288,40 + 5*printList(queryTile.GetComponent<EditorTile>().locksIn).Length,30),"In: " + printList(queryTile.GetComponent<EditorTile>().locksIn), "box");
			if (queryTile.GetComponent<EditorTile>().locksOut.Count > 0)
				GUI.Label (new Rect(Screen.width - 197 - 5*printList(queryTile.GetComponent<EditorTile>().locksOut).Length,(32+5)*(tileList.Length/2+obsList.Length/2)+328,50 + 5*printList(queryTile.GetComponent<EditorTile>().locksOut).Length,30),"Out: " + printList(queryTile.GetComponent<EditorTile>().locksOut), "box");
		}
		
		connectionsBox.List(new Rect(Screen.width - 99,(32+5)*(tileList.Length/2+obsList.Length/2)+240,80,30), new GUIContent(connectionEntry.ToString()), consDropdown, activeButton);
		
		if (connectionEntry != int.Parse (consDropdown[connectionsBox.GetSelectedItemIndex()].text)) {
			conPicked = true;
			connectionEntry = int.Parse (consDropdown[connectionsBox.GetSelectedItemIndex()].text);
			activeSelection = "conn";
			ClearBoxedSelection();
			DrawLinks();
		} else
			conPicked = false;
	
		if (GUI.Button (new Rect(Screen.width - 144,(32+5)*(tileList.Length/2+obsList.Length/2)+240,45,30), "New")) {
			conPicked = true;
			activeSelection = "conn";
			ClearBoxedSelection();
			CheckForEmptyActive();
			DrawLinks();
		}
		
		GUI.Label (new Rect(Screen.width - 147,(32+5)*(tileList.Length/2+obsList.Length/2)+288,141,75), "Lock Groups", "box");
		if (activeSelection == "lock") {
			GUI.DrawTexture (new Rect(Screen.width - 147,(32+5)*(tileList.Length/2+obsList.Length/2)+288,141,75),activeSelBox);
		}
			
		locksBox.List(new Rect(Screen.width - 99,(32+5)*(tileList.Length/2+obsList.Length/2)+320,80,30), new GUIContent(lockEntry.ToString()), locksDropdown, activeButton);
		
		if (lockEntry != int.Parse (locksDropdown[locksBox.GetSelectedItemIndex()].text)) {
			lockPicked = true;
			lockEntry = int.Parse (locksDropdown[locksBox.GetSelectedItemIndex()].text);
			activeSelection = "lock";
			ClearBoxedSelection();
			DrawLinks();
		} else
			lockPicked = false;
		
		if (GUI.Button (new Rect(Screen.width - 144,(32+5)*(tileList.Length/2+obsList.Length/2)+320,45,30), "New")) {
			lockPicked = true;
			activeSelection = "lock";
			ClearBoxedSelection();
			CheckForEmptyActive();
			DrawLinks();
		}
		
		if (queryTile != null) {
			GUI.Label (new Rect(Screen.width - 235, (32+5)*(tileList.Length/2+obsList.Length/2)+370,230,63 + (60 * queryTile.GetComponent<EditorTile>().botMessage[bot].Count)), "Tutorial/Hint Message", "box");
			if (Popup.List (new Rect(Screen.width - 230, (32+5)*(tileList.Length/2+obsList.Length/2)+395,50,30), ref showMessageList, ref bot, messageDropdown[bot], messageDropdown, activeButton)) {
				messPicked = true;
			} else {
				messPicked = false;
			}

			if (GUI.Button(new Rect(Screen.width - 175, (32+5)*(tileList.Length/2+obsList.Length/2)+396,20,20), "+")) {
				if (!queryTile.GetComponent<EditorTile>().botMessage[bot].ContainsKey(keyToAdd)) {
					queryTile.GetComponent<EditorTile>().botMessage[bot].Add (keyToAdd,"");
				}	
			}
			GUI.Label(new Rect(Screen.width - 150, (32+5)*(tileList.Length/2+obsList.Length/2)+393,130,30),"Active at level:");
			keyToAdd = int.Parse(GUI.TextArea(new Rect(Screen.width - 50, (32+5)*(tileList.Length/2+obsList.Length/2)+393,30,30), keyToAdd.ToString(), 2));
			int count = 0;
			List<KeyValuePair<int, string>> tempHold = new List<KeyValuePair<int, string>>(queryTile.GetComponent<EditorTile>().botMessage[bot]);
			foreach (KeyValuePair<int, string> kvp in tempHold) {
				GUI.Label(new Rect(Screen.width - 230, (32+5)*(tileList.Length/2+obsList.Length/2)+425 + (count * 60),30,30), kvp.Key.ToString()+":");
				queryTile.GetComponent<EditorTile>().botMessage[bot][kvp.Key] = GUI.TextArea(new Rect(Screen.width - 200, (32+5)*(tileList.Length/2+obsList.Length/2)+428 + (count * 60),190,60), kvp.Value);
				
				if (GUI.Button(new Rect(Screen.width - 230, (32+5)*(tileList.Length/2+obsList.Length/2)+455 + (count * 60),20,20),"-")) {
					queryTile.GetComponent<EditorTile>().botMessage[bot].Remove(kvp.Key);	
				}
				count++;
			}		
		} else {
			GUI.Label (new Rect(Screen.width - 235, (32+5)*(tileList.Length/2+obsList.Length/2)+370,230,90), "Tutorial/Hint Message", "box");
			GUI.Label (new Rect(Screen.width - 230, (32+5)*(tileList.Length/2+obsList.Length/2)+400,225,60), "Select tile (Left Alt) to set/view messages.");	
		}
		
		if (loadFile) {
			Debug.Log ("Trying to load file loader window...");
			browser.OnGUI ();
			GUI.TextField (new Rect(Screen.width/2-300, 410, 500, 30), browser.CurrentDirectory);
			if (filePath != null) {
				loadFile = false;
				LoadLevel();
				
				consDropdown = new GUIContent[activeCons.Count];
				for (int i = 0; i < consDropdown.Length; i++)
					consDropdown[i] = new GUIContent(activeCons[i].ToString());
			
				locksDropdown = new GUIContent[activeLocks.Count];
				for (int i = 0; i < locksDropdown.Length; i++)
					locksDropdown[i] = new GUIContent(activeLocks[i].ToString());
			}
		}
		
		if (saveFile) {
			browser.OnGUI ();
			fileName = GUI.TextField (new Rect(Screen.width/2-300, 510, 300, 30), fileName);
			GUI.Label (new Rect(Screen.width/2, 510, 40, 30), ".xml");
			if (filePath != null) {
				saveFile = false;
				WriteXML ();
			}
		}
		
		if (GUI.Button (new Rect(390,5,90,60), "Return to Game")) {
			guiInput = true;
			Application.LoadLevel (0);
		}
		
		GUI.EndGroup ();
	}
}

