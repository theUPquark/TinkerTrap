using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : TileClass, Tile {
	
	private Battery bat;
	
	public Generator(int gx, int gy, int tSet) : base(gx, gy, tSet)
	{
		os.PlayOnce(this.GetType ().Name+tSet.ToString()+"_off");
		powered = false;
	}
	
	public override void interact(Obstacle a)
	{
		if (a.GetType () == typeof(Battery) && bat == null) {
			used = true;
			bat = (Battery)a;
		} else if (a.GetType () == typeof(Battery) && bat == (Battery)a) {
			used = true;
			bat = null;
		}
	}
	
	public override void act(List<Obstacle> objs) {
		if (used) {
			if (!powered && bat.charging(this)) {
				os.PlayOnce(this.GetType ().Name+tileSet.ToString()+"_on");
				used = false;
				powered = true;
			} else if (bat == null) {
				os.PlayOnce(this.GetType ().Name+tileSet.ToString()+"_off");
				used = false;
				powered = false;
			}
		}
	}
}
