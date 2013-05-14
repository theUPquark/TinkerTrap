using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : TileClass, Tile {

	private bool open = false;
	private bool botAccess = false;
	private double openUntil = 0.0;
	
	public Door(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		os.frameName = "Door"+tileSet.ToString ()+"_op_00000";
		os.Stop ();
	}
	
	public override bool walkable() {
		if (os.isPlaying)
			return false;
		return open;
	}
	public override void interact(Obstacle a) {
		if (a.GetType() == typeof(Bot3)) {
			Bot3 b3 = (Bot3)a;
			if (b3.charge > 0) {
				botAccess = true;
				openUntil = Time.time+5;
				b3.charge = 0;
			}
		}
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
		if (Time.time >= openUntil) {
			botAccess = false;
		}
	}
	
	public override void act(List<Obstacle> objs) {
		if ((used || botAccess) && !open) {
			os.PlayOnce("Door"+tileSet.ToString ());
			powered = true;
			open = true;
		} else if ((!used && !botAccess) && open) {
			os.PlayOnceBackward("Door"+tileSet.ToString ());
			powered = false;
			open = false;
		}
	}
}