using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : TileClass, Tile {

	public Floor(int gx, int gy) : base(gx, gy)
	{
		frameName = "Ground0";
		os.frameName = frameName;
	}
	
	public override bool walkable {
		get { return true; }
	}
}
