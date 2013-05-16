using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : TileClass, Tile {

	private bool open = false;
	private double openUntil = 0.0;
	private float delayTime = 0f;
	
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
		if (open != powered && delayTime <= Time.time)
			powered = !powered;
	}
	
	public override void act(List<Obstacle> objs) {
		if (used && !open) {
			os.PlayOnce("Door"+tileSet.ToString ());
			open = true;
			delayTime = Time.time + .4f;
		} else if (!used && open) {
			os.PlayOnceBackward("Door"+tileSet.ToString ());
			open = false;
			delayTime = Time.time + .6f;
		}
	}
}