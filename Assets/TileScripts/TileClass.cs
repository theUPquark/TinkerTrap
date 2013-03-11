using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class TileClass : Tile
{
	public bool powered = false;
	public bool locked = false;
	public bool used = false;
	public int wait = 0;
	public bool hold = false;
	public int nextWait;
	public float xiso, yiso;
	public int[] lockGroup;
	public int gridx, gridy;
	public string frameName;
	public GameObject gfx;
	protected OTSprite os;
	private Dictionary<int,List<Tile>> connections = new Dictionary<int, List<Tile>>();
	private Dictionary<int,List<Tile>> locks = new Dictionary<int, List<Tile>>();
	
	public TileClass (int gx, int gy)
	{
		gridx = gx;
		gridy = gy;
		gfx = OT.CreateObject("WorldTiles");
		os = gfx.GetComponent<OTSprite>();
		xiso = (-gy+gx)*GameManager.getTileW();
		yiso = (-gy-gx)*GameManager.getTileW()/2F;
		os.position = new Vector2 (xiso, yiso);
		
		if (walkable)
			os.depth = 1;
	}
	
	public virtual bool walkable {
		get { return false; }
	}
	
	public OTSprite graphic {
		get { return os; }
	}
	
	// Returns the name of the tile
	public string myName()	{
		string tileName = "tile_"+gridx+"_"+gridy;
		return tileName;
	}
	
	public void addConnection(int k, List<Tile> l)
	{
		if (!connections.ContainsKey(k))
		connections.Add (k,l); 	//create key if it didn't exist
		else 
		connections[k] = l;		//replace list if key already exists
	}
	
	public void addLock(int k, List<Tile> l) {
		if (!locks.ContainsKey(k))
		locks.Add (k,l); 	//create key if it didn't exist
		else 
		locks[k] = l;		//replace list if key already exists
	}
	
	// This function detects if the tile is considered active by looking at the powered state of all 'connections'
	// tiles in the group. Things like buttons and pressure plates provide power.
	public bool isActivated() {
		
		// Also add loop to check locks here. All tiles within a lock group must be 'unlocked' for those tiles
		// to remain 'powered,' whereas only one member of a connection group must be active to power the rest.
		
		foreach (List<Tile> conList in locks.Values)
			foreach (Tile t in conList)
				if (((TileClass)t).locked && t != this)
					return false;
		foreach (List<Tile> conList in connections.Values)
			foreach (Tile t in conList)
				if (((TileClass)t).powered)
					return true;
		return false;
	}
	
	public void interact() {
	}
	
	public void update() {
	}
	
	public void act(List<Obstacle> objs) {
	}
}

