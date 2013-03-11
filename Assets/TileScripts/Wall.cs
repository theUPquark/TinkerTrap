using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : TileClass, Tile {

	public Wall(int gx, int gy) : base(gx, gy)
	{
		frameName = "Wall0";
		gfx.GetComponent<OTSprite>().frameName = frameName;
	}
	
}
