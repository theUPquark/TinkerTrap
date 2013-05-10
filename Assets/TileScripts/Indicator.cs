using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Indicator : TileClass, Tile {
	
	private bool active = false;
	
	public Indicator(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		animSelector ();
	}
	
	private void animSelector() {
		if (active) {
			if (tileSet <= 3)
				os.PlayLoop ("Indicator"+tileSet.ToString ()+"_on");
			else
				os.PlayLoop ("Indicator"+(tileSet%4).ToString ()+"_multi_on");
		} else {
			if (tileSet <= 3)
				os.PlayLoop ("Indicator"+tileSet.ToString ()+"_off");
			else
				os.PlayLoop ("Indicator"+(tileSet%4).ToString ()+"_multi_off");
		}
	}
	
	public override bool walkable() {
		return true;
	}
	
	public override void update() {
	}
	
	public override void act(List<Obstacle> objs) {
		if (isActivated () && !active) {
			active = true;
			animSelector ();
		} else if (!isActivated () && active) {
			active = false;
			animSelector ();
		}
	}
}
