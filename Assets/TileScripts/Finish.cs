using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Finish : Floor, Tile {
	
	private bool levelComplete = false;
	
	public Finish(int gx, int gy, int tSet) : base(gx, gy, tSet) {}
	
	public override void act(List<Obstacle> objs) {
		foreach (Obstacle i in objs) {
			if (i.xtile == gridx && i.ytile == gridy)	{
				if (i.GetType() == typeof(Player)){
					//Do a thing
					levelComplete = true;
					break;
				}
			}
		}
	}
	public bool Status () {
		return levelComplete;
	}
}