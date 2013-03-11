using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : TileClass, Tile {

	public Button(int gx, int gy) : base(gx, gy)
	{
		frameName = "WallB";
		locked = true;
		os.frameName = frameName;
	}
	
	public void interact()
	{
		used = true;
	}
	
	public void act(List<Obstacle> objs) {
		if (used) {
			/*if (active)
				//Button off state graphic goes here.
			else
				//Button on state graphic goes here.*/
			powered = !powered;
			used = false;
		}
	}
}
