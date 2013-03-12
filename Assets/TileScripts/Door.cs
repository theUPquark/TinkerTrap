using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : TileClass, Tile {

	private bool open = false;
	private int setBase;
	
	public Door(int gx, int gy, int tSet) : base(gx, gy)
	{
		setBase = tSet;
		frameName = "Door"+setBase.ToString ();
		gfx.GetComponent<OTSprite>().frameName = frameName;
	}
	
	public override bool walkable {
		get { return open; }
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
            os.frameName = "Door"+(setBase+1).ToString ();
			open = true;
			powered = true;
		} else if (!used && walkable) {
            os.frameName = "Door"+(setBase).ToString ();
			open = false;
			powered = false;
		}
	}
}