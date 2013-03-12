using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EditorTile : MonoBehaviour
{
    private int tile = 1;
    private int obs = 0;
    private Dictionary<string,List<int>> connections = new Dictionary<string,List<int>>();
    private Dictionary<string,List<int>> lockGroups = new Dictionary<string,List<int>>();
	
	public int tileType {
		get { return tile; }
		set { tile = value; }
	}
	
	public int obsType {
		get { return obs; }
		set { obs = value; }
	}
	
	public List<int> consIn {
		get {
			if (!connections.ContainsKey ("in"))
				connections.Add("in",new List<int>());
			return connections["in"];
		}
	}
	
	public List<int> consOut {
		get {
			if (!connections.ContainsKey ("out"))
				connections.Add("out",new List<int>());
			return connections["out"];
		}
	}

	public List<int> locksIn {
		get {
			if (!lockGroups.ContainsKey ("in"))
				lockGroups.Add("in",new List<int>());
			return lockGroups["in"];
		}
	}
	
	public List<int> locksOut {
		get {
			if (!lockGroups.ContainsKey ("out"))
				lockGroups.Add("out",new List<int>());
			return lockGroups["out"];
		}
	}
}