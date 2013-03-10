using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EditorTile : MonoBehaviour
{
    private int tile = 1;
    private int obs = 0;
    private List<int> connections = new List<int>();
    private List<int> lockGroups = new List<int>();
	
	void Start() {
	}
	
	public int tileType {
		get { return tile; }
		set { tile = value; }
	}
	
	public int obsType {
		get { return obs; }
		set { obs = value; }
	}
	
	public List<int> consList {
		get { return connections; }
	}
	
	public void setElementConnection (int i) {
		//if (tileData.connections == null)
		//	tileData.connections = new List<int>();
		connections.Add(i);
	}
	
    public void setConnections(string text)
    {
        string[] splitText = text.Split(',');
        if (text.Length > 0)
            connections.Clear();
        foreach (string str in splitText)
            if (str.Length > 0)
                connections.Add(int.Parse(str));
    }

	public List<int> locksList {
		get { return lockGroups; }
	}
	
	public void setElementLock (int i) {
		//if (tileData.lockGroups == null)
		//	tileData.lockGroups = new List<int>();
		lockGroups.Add(i);
	}
	
    public void setLockGroups(string text)
    {
        string[] splitText = text.Split(',');
        if (text.Length > 0)
            lockGroups.Clear();
        foreach (string str in splitText)
            if (str.Length > 0)
                lockGroups.Add(int.Parse(str));
    }
}