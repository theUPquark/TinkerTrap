using UnityEngine;
using System;
using System.Collections.Generic;

public class Tile {
		
	public bool walkable;
	public bool powered = false;
	public bool locked = false;
	public bool used = false;
	public int wait = 0;
	public bool hold = false;
	public int nextWait;
	public float xiso, yiso;
	public int[] lockGroup;
	public int type, gridx, gridy;
	public string frameName;
	public GameObject gfx;
	private Dictionary<int,List<Tile>> connections = new Dictionary<int, List<Tile>>();
	private Dictionary<int,List<Tile>> locks = new Dictionary<int, List<Tile>>();
	
	public Tile (int t, int gx, int gy, GameObject g) {
		type = t;
		gridx = gx;
		gridy = gy;
		gfx = g;
		switch (type) {
		case 2: // Pressure floor tile.
			locked = true;
			walkable = true;
			frameName = "Plate0";
			break;
		case 0: // Plain floor.
			frameName = "Ground0";
			walkable = true;
			break;
		case 8: // Finish zone floor tiles, needs graphic.
			walkable = true;
			frameName = "Ground0";
			break;
		case 4: // Left/Right Door
			walkable = false;
			frameName = "Door2";
			break;
		case 5: // Up/Down Door
			walkable = false;
			frameName = "Door0";
			break;
		case 1: // Plain wall.
			walkable = false;
			frameName = "Wall0";
			break;
		case 3:
		case 6: // Button wall needs 2nd graphic.
			locked = true;
			walkable = false;
			frameName = "WallB";
			break;
		case 7: // "See through" wall (so player is not hidden) needs updated graphic.
			walkable = false;
			frameName = "WallX";
			break;
		}
	}
	
	// Returns the name of the tile
	public string myName()	{
		string tileName = "tile_"+gridx+"_"+gridy;
		return tileName;
	}

	public void addConnection(int k, List<Tile> l) {
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
		
		foreach (List<Tile> conList in locks.Values) {
			foreach (Tile t in conList) {
				if (t.locked) {
					return false;
				}
			}
		}
		foreach (List<Tile> conList in connections.Values) {
			foreach (Tile t in conList) {
				if (t.powered) {
					return true;
				}
			}
		}
		return false;
	}
	
	// Called to update status when activated by the player.
	public void interact() {
		switch (type) {
			case 3:
			case 6:
				used = true;
				break;
		}
		Console.WriteLine ("Tile {0},{1} was just used!",gridx,gridy);
	}
	
	// Called to update status when acted upon by another object.
	public void update() {
		switch (type) {
		case 4:
		case 5:
			if (isActivated())
				used = true;
			else
				used = false;
			break;
		}
	}
	
	// Called with the game Update function.
	public void act(List<Obstacle> objs) {
		
		OTSprite os = gfx.GetComponent<OTSprite>();
		switch (type) {
		default:
			break;
		case 2:
			bool occupied = false;
			foreach (Obstacle i in objs)	
				if (i.xtile == gridx && i.ytile == gridy)	{
					occupied = true;
					break;	//stop once any Obstacle matches Tile
				}
			if (locked == occupied) {
				switch (locked) {
					case true:
						os.frameName = "Plate1";
						break;
					case false:
						os.frameName = "Plate0";
						break;
				}
				locked = !locked;
				powered = !powered;
			}
			break;
		case 3:
		case 6:
			if (used) {
				/*if (active)
					//Button off state graphic goes here.
				else
					//Button on state graphic goes here.*/
				Console.WriteLine ("Button at {0},{1} was pushed!",gridx,gridy);
				powered = !powered;
				used = false;
			}
			break;
		case 4: //Is there an easy way to consolidate 4 & 5? Maybe an AnimationSprite with frames?
			if (used && !walkable) {
				Console.WriteLine ("Door used!");
                os.frameName = "Door3";
				walkable = true;
				powered = true;
			} else if (!used && walkable) {
                os.frameName = "Door2";
				walkable = false;
				powered = false;
			}
			break;
		case 5:
			if (used && !walkable) {
				Console.WriteLine ("Door used!");
                os.frameName = "Door1";
				walkable = true;
				powered = true;
			} else if (!used && walkable) {
                os.frameName = "Door0";
				walkable = false;
				powered = false;
			}
			break;
		}
	}
}

