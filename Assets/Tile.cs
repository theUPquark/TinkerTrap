using UnityEngine;

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
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				walkable = false;
				type = 1;
				break;
			}
		}
	}
}

