using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : TileClass, Tile {

	public Floor(int gx, int gy, int tSet) : base(gx, gy, tSet) {}
	
	public override bool walkable() {
		return true;
	}
}
