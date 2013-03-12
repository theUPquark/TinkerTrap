using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Floor, Tile {

	public Plate(int gx, int gy) : base(gx, gy)
	{
		frameName = "Plate0";
		locked = true;
		gfx.GetComponent<OTSprite>().frameName = frameName;
	}
	
	public override void act(List<Obstacle> objs) {
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
	}
}
