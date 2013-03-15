using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Electrified : Floor, Tile {
	
	bool on = false;
	
	public Electrified(int gx, int gy, int tSet) : base(gx, gy, tSet)
	{
		os.PlayLoop ("Electrified"+tSet.ToString()+"_off");
	}
	
	public bool walkable(Obstacle b)
	{
		if (on) {
			if (b.GetType() == typeof(Box) || b.GetType() == typeof(Bot2))
				return true;
			else
				return false;
		}
		return true;
	}
	
	public virtual bool isActivated() {
		foreach (List<Tile> conList in locks[1].Values)
			foreach (Tile t in conList)
				if (!((TileClass)t).locked && t != this)
					return false;
		foreach (List<Tile> conList in connections[1].Values)
			foreach (Tile t in conList)
				if (((TileClass)t).powered)
					return true;
		return false;
	}
	
	public override void update ()
	{
		if (isActivated ())
			on = true;
		else
			on = false;
	}
	
	public override void act(List<Obstacle> objs) {
		switch (on) {
			case true:
				os.PlayLoop ("Electrified"+tileSet.ToString()+"_on");
				break;
			case false:
				os.PlayLoop ("Electrified"+tileSet.ToString()+"_off");
				break;
		}
		/*bool occupied = false;
		foreach (Obstacle i in objs)	
			if (i.xtile == gridx && i.ytile == gridy)	{
				occupied = true;
				break;	//stop once any Obstacle matches Tile
			}
		if (locked == occupied) {
			locked = !locked;
			powered = !powered;
		}*/
	}
}
