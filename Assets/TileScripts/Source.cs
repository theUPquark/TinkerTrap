using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Source : TileClass, Tile {
	
	public Source(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		powered = true;
		os.PlayOnce ("Wall0");
	}
	
}
