using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : TileClass, Tile {

	public Button(int gx, int gy, int tSet) : base(gx, gy, tSet)
	{
		os.PlayOnce(this.GetType ().Name+tSet.ToString());
		locked = true;
	}
	
	public override void interact()
	{
		used = true;
	}
	
	public override void act(List<Obstacle> objs) {
		if (used) {
			/*if (active)
				//Button off state graphic goes here.
			else
				//Button on state graphic goes here.*/
			/*if (isActivated ()) {
				foreach(List<Tile> i in connections[1].Values)
					foreach(Tile j in i)
						j.interact ();*/
				used = false;
				powered = !powered;
			//}
		}
	}
}
