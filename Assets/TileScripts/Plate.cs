using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plate : Floor, Tile {

	public Plate(int gx, int gy, int tSet) : base(gx, gy, tSet)
	{
		os.PlayLoop ("Plate"+tSet.ToString()+"_off");
		locked = true;
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
					os.PlayLoop ("Plate"+tileSet.ToString()+"_on");
					break;
				case false:
					os.PlayLoop ("Plate"+tileSet.ToString()+"_off");
					break;
			}
			locked = !locked;
			powered = !powered;
		}
	}
}
