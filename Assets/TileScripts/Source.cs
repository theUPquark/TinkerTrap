using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Source : TileClass, Tile {
	
	public Source(int gx, int gy, int tSet) : base(gx, gy, tSet) {
		powered = true;
		os.PlayOnce ("Wall0");
	}
	
	public override bool isActivated ()
	{
		foreach (List<Tile> conList in locks[0].Values)
			foreach (Tile t in conList)
				if (((TileClass)t).locked && t != this)
					return false;
		return true;
	}
	
	public override void update ()
	{
		if (isActivated ())
			powered = true;
		else
			powered = false;
	}
}
