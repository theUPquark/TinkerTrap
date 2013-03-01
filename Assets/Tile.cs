using UnityEngine;
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
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
		public GameObject gfx;
		private Dictionary<int,List<AssemblyCSharp.Tile>> connections = new Dictionary<int, List<AssemblyCSharp.Tile>>();
		
		public Tile (int t, int gx, int gy, GameObject g) {
			gridx = gx;
			gridy = gy;
			gfx = g;
			switch (t) {
	/*			case 2:
					locked = true;
				case 0:
				case 8:
					walkable = true;
					break;
				case 4:
				case 5:
				case 1:
				case 3:
				case 6:
				case 7:
					walkable = false;
					break;*/
			case 0:
			case 2:
			case 8:
				walkable = true;
				type = 2;
				break;
			case 4:
			case 5:
				walkable = false;
				type = 0;
				break;
			case 1:
			case 6:
				walkable = false;
				type = 3;
				break;
			case 3:
				walkable = false;
				type = 4;
				break;
			case 7:
				walkable = false;
				type = 5;
				break;
			}
		}
	
		public void addConnection(int k, List<AssemblyCSharp.Tile> l) {
			connections.Add (k,l);
		}
		
		// This function detects if the tile is considered active by looking at the powered state of all 'connections'
		// tiles in the group. Things like buttons and pressure plates provide power.
		public bool isActivated() {
			
			// Also add loop to check locks here. All tiles within a lock group must be 'unlocked' for those tiles
			// to remain 'powered,' whereas only one member of a connection group must be active to power the rest.
			
			foreach (List<AssemblyCSharp.Tile> conList in connections.Values) {
				foreach (AssemblyCSharp.Tile t in conList) {
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
		public void act(List<AssemblyCSharp.Obstacle> objs) {
			OTSprite os = gfx.GetComponent<OTSprite>();
			switch (type) {
				default:
					break;
				case 2:
					bool occupied = false;
					foreach (AssemblyCSharp.Obstacle i in objs)
						if (i.xtile == gridx && i.ytile == gridy)
							occupied = true;
					if (locked == occupied) {
						switch (locked) {
							case true:
								//On state graphic for pressure plate goes here.
								break;
							case false:
								//Off state graphic for pressure plate goes here.
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
						powered = !powered;
						used = false;
					}
					break;
				case 4:
				case 5:
					if (used && !walkable) {
                        os.frameName = "Door1";
						os.frameIndex = 1;
						walkable = true;
					} else if (!used && walkable) {
                        os.frameName = "Door0";
						os.frameIndex = 0;
						walkable = false;
					}
					break;
			}
		}
	}
}

