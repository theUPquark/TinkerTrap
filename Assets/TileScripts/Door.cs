using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : TileClass, Tile {

	private bool open = false;
	
	public Door(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		os.frameName = "Door"+tileSet.ToString ()+"_op_00000";
		os.Stop ();
	}
	
	public override bool walkable() {
		if (os.isPlaying)
			return false;
		return open;
	}
	
	public override bool walkable (Obstacle o)
	{
		if (o.GetType () == typeof(Box))
			return false;
		return this.walkable ();
	}
	
	public override void update() {
		if (isActivated()) {
			used = true;
		} else {
			used = false;
		}
	}
	
	public override void act(List<Obstacle> objs) {
		if (used && !open) {
			os.PlayOnce("Door"+tileSet.ToString ());
			powered = true;
			open = true;
		} else if (!used && open) {
			os.PlayOnceBackward("Door"+tileSet.ToString ());
			powered = false;
			open = false;
		}
	}
}