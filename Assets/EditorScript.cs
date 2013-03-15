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
	private string[] tileList = new string[] {"Wall","Floor","Door","Button","Plate","Electrified", "Generator", "Source"};
	private string[] obsList = new string[] {"Spawn", "Box", "Battery"};
	private string activeSelection = "";
	private int activeSet = 0;
	private int gridW = 10;
	private int gridH = 10;
	private string tempW = "10";
	private string tempH = "10";
	private List<int> activeCons = new List<int>();
	private List<int> activeLocks = new List<int>();
	
	
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
	
	// For connection/lock selection dropdowns
	
	private bool showConList = false;
	private bool showLockList = false;
	private int connectionEntry = 0;
	private int lockEntry = 0;
	private GUIContent[] consDropdown = {new GUIContent("1"),new GUIContent("2"),new GUIContent("3")};
	private GUIContent[] locksDropdown = {new GUIContent("1"),new GUIContent("2"),new GUIContent("3")};
	private bool conPicked = false;
	private bool lockPicked = false;
	
	// Drawing lines
	
	private bool validAnchor = false; // Control variable to help set tracer line
	private Vector3 anchor;
	private GameObject anchorYX;
	
	private GameObject line;
	// Use this for initialization
	void Start () {
		line = GameObject.Find ("mouseLine");
		SetGrid();
		
		// Make some content for the popup list
//		consDropdown = new GUIContent[0];
//		
//		locksDropdown = new GUIContent[0];
	}
	
	private Vector3 ReturnTileCenter (Vector3 v)
	{
		Vector3 change = new Vector3(v.x+16,v.y-16,v.z-10);
		return change;
	}
	
	private void DrawVerticies(Vector3 a, Vector3 b)
	{
		line.GetComponent<LineRenderer>().SetPosition(0, a);
		line.GetComponent<LineRenderer>().SetPosition(1, b);
	}
	
	private void DrawConnections(int num)
	{
		foreach (List<GameObject> g in map)
		{
			foreach (GameObject o in g)
			{
				o.GetComponent<LineRenderer>().SetColors (new Color(197f/255f,244f/255f,184f/255f), new Color(22f/255f,148f/255f,64f/255f));
				// Checking all tiles for num, using consOut
				if ( o.GetComponent<EditorTile>().consOut.Contains(num))
				{
					int count = 0;
					o.GetComponent<LineRenderer>().SetVertexCount(3);
					o.GetComponent<LineRenderer>().SetPosition(count,ReturnTileCenter(o.transform.position));
					foreach (List<GameObject> g2 in map)
					{
						foreach (GameObject o2 in g2)
						{
							// Check all tiles again for num, using consIn
							if (o2.GetComponent<EditorTile>().consIn.Contains(num))
							{
								// With another matching tile, set another LineRender point, and then return to source tile again
								o.GetComponent<LineRenderer>().SetVertexCount(count + 3);
								o.GetComponent<LineRenderer>().SetPosition(++count,ReturnTileCenter(o2.transform.position));
								o.GetComponent<LineRenderer>().SetPosition(++count,ReturnTileCenter(o.transform.position));
							}
						}
					}
				}
				else {
					o.GetComponent<LineRenderer>().SetVertexCount(1);
					o.GetComponent<LineRenderer>().SetPosition(0,o.transform.position);
				}
			}
		}
	}
	
	private void DrawLocks(int num)
	{
		foreach (List<GameObject> g in map)
		{
			foreach (GameObject o in g)
			{
				o.GetComponent<LineRenderer>().SetColors (new Color(236f/255f,243f/255f,183f/255f,255f/255f), new Color(106f/255f,58f/255f,32f/255f,255f/255f));
				// Checking all tiles for num, using locksOut
				if ( o.GetComponent<EditorTile>().locksOut.Contains(num))
				{
					int count = 0;
					o.GetComponent<LineRenderer>().SetVertexCount(3);
					o.GetComponent<LineRenderer>().SetPosition(count,ReturnTileCenter(o.transform.position));
					foreach (List<GameObject> g2 in map)
					{
						foreach (GameObject o2 in g2)
						{
							// Check all tiles again for num, using locksin
							if (o2.GetComponent<EditorTile>().locksIn.Contains(num))
							{
								// With another matching tile, set another LineRender point, and then return to source tile again
								o.GetComponent<LineRenderer>().SetVertexCount(count + 3);
								o.GetComponent<LineRenderer>().SetPosition(++count,ReturnTileCenter(o2.transform.position));
								o.GetComponent<LineRenderer>().SetPosition(++count,ReturnTileCenter(o.transform.position));
							}
						}
					}
				}
				else {
					o.GetComponent<LineRenderer>().SetVertexCount(1);
					o.GetComponent<LineRenderer>().SetPosition(0,o.transform.position);
				}
			}
		}
	}
	
	private void SetGrid() {
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
					map[i][j].AddComponent<LineRenderer>();
    				map[i][j].GetComponent<LineRenderer>().material = new Material (Shader.Find("Particles/Additive"));
					map[i][j].GetComponent<LineRenderer>().SetWidth (6f, 2f);
					map[i][j].GetComponent<OTSprite>().position = new Vector2(j*32f,i*-32f);
					mapObs[i][j].GetComponent<OTSprite>().position = new Vector2(j*32f,i*-32f);
					map[i][j].GetComponent<OTSprite>().frameName = "ed_Wall0";
					mapObs[i][j].GetComponent<OTSprite>().frameName = "empty";
					mapObs[i][j].GetComponent<OTSprite>().depth = -1;
				}
			}
			if (gridW < map[i].Count) {
				foreach (GameObject t in map[i].GetRange (gridW,map[i].Count-gridW))
					Destroy (t);
				foreach (GameObject o in mapObs[i].GetRange (gridW,map[i].Count-gridW))
					Destroy (o);
				map[i].RemoveRange (gridW,map[i].Count-gridW);
				mapObs[i].RemoveRange (gridW,mapObs[i].Count-gridW);
			}
		}
		if (gridH < map.Count) {
			foreach (List<GameObject> l in map.GetRange (gridH,map.Count-gridH))
				foreach (GameObject t in l)
					Destroy (t);
			foreach (List<GameObject> l in mapObs.GetRange (gridH,map.Count-gridH))
				foreach (GameObject o in l)
					Destroy (o);
			map.RemoveRange (gridH,map.Count-gridH);
			mapObs.RemoveRange (gridH,map.Count-gridH);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (tileList.Contains (activeSelection) && Input.GetMouseButton (0) && !guiError && !loadFile && !saveFile && !guiInput)
		{
			Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
			int selectX = (int)(Math.Floor (mouseLocation.x/32));
			int selectY = (int)(Math.Floor (mouseLocation.y/-32));
			if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
				SetTypeByDraw(map[selectY][selectX]);
				SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
			}
		} else if (obsList.Contains (activeSelection) && Input.GetMouseButton (0) && !guiError && !loadFile && !saveFile && !guiInput) {
			Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
			int selectX = (int)(Math.Floor (mouseLocation.x/32));
			int selectY = (int)(Math.Floor (mouseLocation.y/-32));
			if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
				SetObsByDraw(map[selectY][selectX]);
				SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
			}
		} else if (activeSelection == "empty" && Input.GetMouseButton (0) && !guiError && !loadFile && !saveFile && !guiInput) {
			Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
			int selectX = (int)(Math.Floor (mouseLocation.x/32));
			int selectY = (int)(Math.Floor (mouseLocation.y/-32));
			if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
				map[selectY][selectX].GetComponent<EditorTile>().obsType = "";
				SetGraphics(map[selectY][selectX], mapObs[selectY][selectX]);
			}
		} else if (Input.GetMouseButtonDown (0) && !guiError && !loadFile && !saveFile && !guiInput) {
			Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
			int selectX = (int)(Math.Floor (mouseLocation.x/32));
			int selectY = (int)(Math.Floor (mouseLocation.y/-32));
			if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
				validAnchor = true;
				anchorYX = map[selectY][selectX];
				anchor = ReturnTileCenter(map[selectY][selectX].transform.position);
			}
		} else {
			// Deletes node on right click. Lone nodes remain.
			if (Input.GetMouseButtonDown (1) && !guiError && !loadFile && !saveFile && !guiInput)
			{
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				int selectX = (int)(Math.Floor (mouseLocation.x/32));
				int selectY = (int)(Math.Floor (mouseLocation.y/-32));
				if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
					if (activeSelection == "conn") {
						if (map[selectY][selectX].GetComponent<EditorTile>().consIn.Contains(connectionEntry))
							map[selectY][selectX].GetComponent<EditorTile>().consIn.Remove(connectionEntry);
						if (map[selectY][selectX].GetComponent<EditorTile>().consOut.Contains(connectionEntry))
							map[selectY][selectX].GetComponent<EditorTile>().consOut.Remove(connectionEntry);
						DrawConnections(connectionEntry);
					} else {
						if (map[selectY][selectX].GetComponent<EditorTile>().locksIn.Contains(lockEntry))
							map[selectY][selectX].GetComponent<EditorTile>().locksIn.Remove(lockEntry);
						if (map[selectY][selectX].GetComponent<EditorTile>().locksOut.Contains(lockEntry))
							map[selectY][selectX].GetComponent<EditorTile>().locksOut.Remove(lockEntry);
						DrawLocks(lockEntry);
					}
				}
			}
			if (Input.GetMouseButton (0) && !guiError && !loadFile && !saveFile && !guiInput && validAnchor == true)
			{
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				DrawVerticies(anchor,mouseLocation);
			}
			if (Input.GetMouseButtonUp (0) && !guiError && !loadFile && !saveFile && !guiInput)
			{
				Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
				int selectX = (int)(Math.Floor (mouseLocation.x/32));
				int selectY = (int)(Math.Floor (mouseLocation.y/-32));
				validAnchor = false;
				if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
					// Set the connection (20) or lockgroup (21)
					if (activeSelection == "conn" && map[selectY][selectX] != anchorYX) // Don't add nodes if they start/end on same tile
					{	if (!map[selectY][selectX].GetComponent<EditorTile>().consIn.Contains(connectionEntry)) //Don't add node if it already exists
							map[selectY][selectX].GetComponent<EditorTile>().consIn.Add(connectionEntry);
						if (!anchorYX.GetComponent<EditorTile>().consOut.Contains(connectionEntry))
							anchorYX.GetComponent<EditorTile>().consOut.Add(connectionEntry);
						DrawConnections(connectionEntry);
					} 
					else if (activeSelection == "lock" && map[selectY][selectX] != anchorYX)
					{	if (!map[selectY][selectX].GetComponent<EditorTile>().locksIn.Contains(lockEntry))
							map[selectY][selectX].GetComponent<EditorTile>().locksIn.Add(lockEntry);
						if (!anchorYX.GetComponent<EditorTile>().locksOut.Contains(lockEntry))
							anchorYX.GetComponent<EditorTile>().locksOut.Add (lockEntry);
						
						DrawLocks(lockEntry);
					}
				}
				DrawVerticies(anchor,anchor); // Removing mouse line from view
			}
		}
	}
	
	void LateUpdate () {
			transform.position = new Vector3(camera.orthographicSize*((float)(Screen.width)/(float)(Screen.height))-20, -camera.orthographicSize*(1-150f/Screen.height), transform.position.z);
	}
	
	private void SetTypeByDraw(GameObject a)
	{
		a.GetComponent<EditorTile>().tileType = activeSelection;
		a.GetComponent<EditorTile>().tileSet = activeSet;
	}
	
	private void SetObsByDraw(GameObject a)
	{
		a.GetComponent<EditorTile>().obsType = activeSelection;
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
					}
				}
			}
			read.Close ();
			filePath = null;
		}
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
			GUI.Label (new Rect (5, 70, 100, 60), guiErS);
			if (GUI.Button (new Rect(5, 135, 60, 30), "Okay.")) {
				guiInput = true;
				guiError = false;
				guiErS = "";
			}
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-10,5,50,30), "Tiles:");
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
			}
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-10,(32+5)*(tileList.Length/2+1)+40,70,30), "Spawns:");
		for (int i = 0; i < obsList.Length+1; i++) {
			Texture tex;
			int oSet = 0;
			GUIStyle buttonStyle;
			if (i == obsList.Length) {
				buttonStyle = passiveButton;
				tex = (Texture) Resources.Load ("Editor/empty", typeof(Texture));
				if (GUI.Button (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length+i+4)/2)+40,32,32), tex, buttonStyle)) {
					guiInput = true;
					activeSelection = "empty";
				}
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
				if (GUI.Button (new Rect(Screen.width-(32*(2-(i+1)%2))-10,(32+5)*((tileList.Length+i+4)/2)+40,32,32), tex, buttonStyle)) {
					guiInput = true;
					activeSelection = obsList[i];
					activeSet = oSet;
				}
			}
		}
		
		if (GUI.Button (new Rect(200,5,90,60), "Save") && (!saveFile && !loadFile) ) { // Changed '||' to '&&' so filebrower won't open both load and save windows
			guiInput = true;
			saveFile = true;
			browser = BrowserSetup ();
		}
		
		if (GUI.Button (new Rect(295,5,90,60), "Load") && (!saveFile && !loadFile) ) { // Changed '||' to '&&' so filebrower won't open both load and save windows
			guiInput = true;
			loadFile = true;
			browser = BrowserSetup ();
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-40,(32+5)*(tileList.Length/2+obsList.Length/2)+200,90,30), "Connections:");
	
		if (Popup.List (new Rect(Screen.width-(32*2)-40,(32+5)*(tileList.Length/2+obsList.Length/2)+240,90,30), ref showConList, ref connectionEntry, new GUIContent(connectionEntry.ToString()), consDropdown, activeButton)) {
			conPicked = true;
			connectionEntry = int.Parse (consDropdown[connectionEntry].text);
			DrawConnections (connectionEntry);
			activeSelection = "conn";
		} else
			conPicked = false;
	
		if (GUI.Button (new Rect(Screen.width-(32*2)-80,(32+5)*(tileList.Length/2+obsList.Length/2)+240,40,30), "Add")) {
//			activeSelection = 20;
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-40,(32+5)*(tileList.Length/2+obsList.Length/2)+280,150,30), "Lock Groups:");
		
		if (Popup.List (new Rect(Screen.width-(32*2)-40,(32+5)*(tileList.Length/2+obsList.Length/2)+320,90,30), ref showLockList, ref lockEntry, new GUIContent(lockEntry.ToString()), locksDropdown, activeButton)) {
			lockPicked = true;
			lockEntry = int.Parse (locksDropdown[lockEntry].text);
			DrawLocks(lockEntry);
			activeSelection = "lock";
		} else
			lockPicked = false;
		
		if (GUI.Button (new Rect(Screen.width-(32*2)-80,(32+5)*(tileList.Length/2+obsList.Length/2)+320,40,30), "Add")) {
//			activeSelection = 21;
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

