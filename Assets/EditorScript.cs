using UnityEngine;
using System;
using System.Collections.Generic;

public class EditorScript : MonoBehaviour {
	
	private List<List<GameObject>> map = new List<List<GameObject>>();
	private int activeSelection = 0;
	private int gridW = 10;
	private int gridH = 10;
	private string tempW = "10";
	private string tempH = "10";
	private bool guiError = false;
	private string guiErS = "";
	
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
					map[i][j].GetComponent<OTSprite>().position = new Vector2(34F*j,-34F*i);
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
	void Update () {
		//transform.localPosition = new Vector3(0+Screen.width/2F, 0+Screen.height/2-40, transform.localPosition.z);
	}
	
	/*private void WriteXML()
    {
        // Transfer multidimensional array to reg array, because XML doesn't support it otherwise. >:(
        ButtonAndTile.variables[] tempArray = new ButtonAndTile.variables[columns * rows];
        int step = 0;
        for (int j = 0; j < columns; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                tempArray[step++] = new ButtonAndTile.variables(tileSheet[j, i].tileData);
            }
        }

        string fileName = textBoxFileName.Text + ".xml" ;
        System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ButtonAndTile.variables[]));
        System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);

        writer.Serialize(file, tempArray);
        file.Close();
    }*/
	
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
			if (GUI.Button (new Rect(Screen.width-(32*(2-i%2))-10,(32+5)*(i/2)+40,32,32), buttonGfx[i], buttonStyle) == true) {
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
			if (GUI.Button (new Rect(Screen.width-(32*(2-(i+3)%2))-10,(32+5)*((i+3)/2)+40,32,32), buttonGfx[i], buttonStyle) == true) {
				activeSelection = i;
			}
		}
		GUI.EndGroup ();
	}
}
