using UnityEngine;
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Tile {
			
		public bool walkable;
		public bool active = false;
		public bool locked = false;
		public bool used = false;
		public int wait = 0;
		public bool hold = false;
		public int nextWait;
//		public int depth;
		public float xiso, yiso;
		public int[] connection;
		public int[] lockGroup;
		public int type, gridx, gridy;
		public GameObject gfx;
		private Dictionary<int,List<AssemblyCSharp.Tile>> connections = new Dictionary<int, List<AssemblyCSharp.Tile>>();
		
		public Tile (int t, int gx, int gy, GameObject g) {
			type = t;
			gridx = gx;
			gridy = gy;
			gfx = g;
			switch (type) {
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
				type = 0;
				break;
			case 1:
			case 4:
			case 5:
			case 6:
				walkable = false;
				type = 1;
				break;
			case 3:
				walkable = false;
				type = 2;
				break;
			case 7:
				walkable = false;
				type = 3;
				break;
			}
		}
	
		public void addConnection(int k, List<AssemblyCSharp.Tile> l) {
			connections.Add (k,l);
		}
		
		public int[] interact() {
			switch (type) {
				default:
					return new int[] {0};
					break;
				case 3:
				case 6:
					int[] keylist = new int[connections.Count];
					int i = 0;
					foreach (int k in connections.Keys) {
						keylist[i] = k;
						i++;
					}
					return keylist;
					break;
			}
		}
	}
}

