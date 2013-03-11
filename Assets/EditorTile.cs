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
	
	void Start() {
		connections.Add("in",new List<int>());
		connections.Add("out",new List<int>());
	}
	
	public int tileType {
		get { return tile; }
		set { tile = value; }
	}
	
	public int obsType {
		get { return obs; }
		set { obs = value; }
	}
	
	public List<int> consIn {
		get { return connections["in"]; }
	}
	
	public List<int> consOut {
		get { return connections["out"]; }
	}

	public List<int> locksIn {
		get { return lockGroups["in"]; }
	}
	
	public List<int> locksOut {
		get { return lockGroups["out"]; }
	}
}