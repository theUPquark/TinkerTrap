using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : TileClass, Tile {

	private bool open = false;
	
	public Door(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		os.PlayOnce("Door"+tSet.ToString ()+"_cl");
	}
	
	public override bool walkable() {
		return open;
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
			os.PlayOnce("Door"+tileSet.ToString ()+"_op");
			open = true;
			powered = true;
		} else if (!used && open) {
			os.PlayOnce("Door"+tileSet.ToString ()+"_cl");
			open = false;
			powered = false;
		}
	}
}