using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;

public class EditorScript : MonoBehaviour {
	
	private List<List<GameObject>> map = new List<List<GameObject>>();
	private int activeSelection = 0;
	private int gridW = 10;
	private int gridH = 10;
	private string tempW = "10";
	private string tempH = "10";
	private bool guiError = false;
	private bool loadFile = false;
	private bool saveFile = false;
	private FileBrowser browser;
	private string guiErS = "";
	protected string filePath;
	protected string fileName = "level";
	
	public GUIStyle activeButton;
	public GUIStyle passiveButton;
	public GUISkin mainSkin;
	public Texture2D[] buttonGfx;
	
	// Use this for initialization
	void Start () {
		SetGrid();
	}
	
	private void SetGrid() {
		for (int i = 0; i < gridH; i++) {
			if (map.Count == i)
				map.Add (new List<GameObject>());
			for (int j = 0; j < gridW; j++) {
				if (map[i].Count == j) {
					map[i].Add(OT.CreateObject ("builderSprite"));
					map[i][j].AddComponent<EditorTile>();
					map[i][j].GetComponent<OTSprite>().position = new Vector2(j*32f,i*-32f);
					map[i][j].GetComponent<OTSprite>().frameName = "wall";
				}
			}
			if (gridW < map[i].Count) {
				foreach (GameObject t in map[i].GetRange (gridW,map[i].Count-gridW))
					Destroy (t);
				map[i].RemoveRange (gridW,map[i].Count-gridW);
			}
		}
		if (gridH < map.Count) {
			foreach (List<GameObject> l in map.GetRange (gridH,map.Count-gridH))
				foreach (GameObject t in l)
					Destroy (t);
			map.RemoveRange (gridH,map.Count-gridH);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButton (0))
		{
			Vector3 mouseLocation = camera.ScreenToWorldPoint (Input.mousePosition);
			int selectX = (int)(Math.Floor (mouseLocation.x/32));
			int selectY = (int)(Math.Floor (mouseLocation.y/-32));
			if ((selectY >= 0 && selectY < gridH) && (selectX >= 0 && selectX < gridW)) {
				SetTypeByUI(map[selectY][selectX]);
				SetGraphics(map[selectY][selectX]);
			}
		}
	}
	
	void LateUpdate () {
			transform.position = new Vector3(camera.orthographicSize*((float)(Screen.width)/(float)(Screen.height))-20, -camera.orthographicSize*(1-150f/Screen.height), transform.position.z);
	}
	
	private void SetTypeByUI(GameObject a)
	{
		switch (activeSelection) {
		case 0:
			a.GetComponent<EditorTile>().tileType = 1;
			break;
		case 1:
			a.GetComponent<EditorTile>().tileType = 0;
			break;
		case 2:
			a.GetComponent<EditorTile>().tileType = 3;
			break;
		case 3:
			a.GetComponent<EditorTile>().tileType = 2;
			break;
		case 4:
			a.GetComponent<EditorTile>().tileType = 4;
			break;
		case 5:
			a.GetComponent<EditorTile>().tileType = 5;
			break;
		case 6:
			a.GetComponent<EditorTile>().tileType = 7;
			break;
		case 7:
			a.GetComponent<EditorTile>().obsType = 1;
			break;
		case 8:
			a.GetComponent<EditorTile>().obsType = 4;
			break;
		}
	}
	
	private void SetGraphics(GameObject a)
	{
		switch (a.GetComponent<EditorTile>().tileType) {
		case 1:
			a.GetComponent<OTSprite>().frameName = "wall";
			break;
		case 0:
			a.GetComponent<OTSprite>().frameName = "floor";
			break;
		case 3:
			a.GetComponent<OTSprite>().frameName = "button";
			break;
		case 2:
			a.GetComponent<OTSprite>().frameName = "plate";
			break;
		case 4:
			a.GetComponent<OTSprite>().frameName = "doorRL";
			break;
		case 5:
			a.GetComponent<OTSprite>().frameName = "doorUD";
			break;
		case 7:
			a.GetComponent<OTSprite>().frameName = "dangerFloor";
			break;
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
		filePath = path;
	}
	
	private void ReturnSavePath(string path)
	{
		filePath = path + "/" + fileName + ".xml";
	}
	
	private void WriteXML()
    {
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		using (XmlWriter writer = XmlWriter.Create(filePath,settings)) {
			writer.WriteStartDocument ();
			writer.WriteStartElement ("document");
			writer.WriteElementString ("width", gridW.ToString ());
			writer.WriteElementString ("height", gridH.ToString ());
			foreach (List<GameObject> i in map) {
				writer.WriteStartElement ("y");
				foreach (GameObject j in i) {
					writer.WriteStartElement ("x");
					writer.WriteElementString ("type",j.GetComponent<EditorTile>().tileType.ToString ());
					writer.WriteElementString ("obs",j.GetComponent<EditorTile>().obsType.ToString ());
					writer.WriteStartElement ("connections");
					foreach (int cons in j.GetComponent<EditorTile>().consList) {
						writer.WriteValue (cons);
					}
					writer.WriteEndElement ();
					writer.WriteStartElement ("locks");
					foreach (int locks in j.GetComponent<EditorTile>().locksList) {
						writer.WriteValue (locks);
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
		using (XmlReader read = XmlReader.Create (filePath, settings)) {
			int i = -1;
			int j = -1;
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
							SetGraphics(map[j][i]);
						j++;
						i = -1;
						Console.WriteLine (j.ToString());
						break;
					case "x":
						if (i >= 0)
							SetGraphics(map[j][i]);
						else
							SetGraphics(map[j][gridW-1]);
						Console.WriteLine (i.ToString());
						i++;
						break;
					case "type":
						read.Read ();
						map[j][i].GetComponent<EditorTile>().tileType = int.Parse (read.Value);
						break;
					case "obs":
						read.Read ();
						map[j][i].GetComponent<EditorTile>().obsType = int.Parse (read.Value);
						break;
					//case "connections":
					//case "locks":
					}
				}
			}
			filePath = null;
		}
	}
	
	void OnGUI () {
		GUI.skin = mainSkin;
		GUI.BeginGroup (new Rect(0,0,Screen.width,Screen.height));
		GUI.Label (new Rect(5,5,60,30), "Width:");
		GUI.Label (new Rect(70,5,60,30), "Height:");
		tempW = GUI.TextField (new Rect(5,35,30,30), tempW, 2);
		tempH = GUI.TextField (new Rect(70,35,30,30), tempH, 2);
		if (GUI.Button (new Rect(135,35,60,30), "Apply")) {
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
				guiError = false;
				guiErS = "";
			}
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-10,5,50,30), "Tiles:");
		for (int i = 0; i < buttonGfx.Length-2; i++) {
			GUIStyle buttonStyle;
			if (i == activeSelection)
				buttonStyle = activeButton;
			else
				buttonStyle = passiveButton;
			if (GUI.Button (new Rect(Screen.width-(32*(2-i%2))-10,(32+5)*(i/2)+40,32,32), buttonGfx[i], buttonStyle)) {
				activeSelection = i;
			}
		}
		
		GUI.Label (new Rect(Screen.width-(32*2)-10,(32+5)*(buttonGfx.Length/2)+40,70,30), "Spawns:");
		for (int i = buttonGfx.Length-2; i < buttonGfx.Length; i++) {
			GUIStyle buttonStyle;
			if (i == activeSelection)
				buttonStyle = activeButton;
			else
				buttonStyle = passiveButton;
			if (GUI.Button (new Rect(Screen.width-(32*(2-(i+3)%2))-10,(32+5)*((i+3)/2)+40,32,32), buttonGfx[i], buttonStyle)) {
				activeSelection = i;
			}
		}
		
		if (GUI.Button (new Rect(200,5,90,60), "Save") && (!saveFile || !loadFile) ) {
			saveFile = true;
			browser = BrowserSetup ();
		}
		
		if (GUI.Button (new Rect(295,5,90,60), "Load") && (!saveFile || !loadFile) ) {
			loadFile = true;
			browser = BrowserSetup ();
		}
		
		if (loadFile) {
			browser.OnGUI ();
			GUI.TextField (new Rect(Screen.width/2-300, 410, 500, 30), browser.CurrentDirectory);
			if (filePath != null) {
				loadFile = false;
				LoadLevel();
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
			Application.LoadLevel (0);
		}
		
		GUI.EndGroup ();
	}
}

