using System;
using System.Collections.Generic;
using UnityEngine;

public class Hands : ObstacleClass, Obstacle
{
	private Bot1 b1;
	public Obstacle grabbed;
	public bool grabbing;
	
	public Hands (Bot1 refBot1) : base(0)
	{
		b1 = refBot1;
		os.visible = false;
		
		setX (posX);
		setY (posY);
		
		
	}
	
	public override int width {
		get { return 70; }
	}
	
	public void PlaceHands () {
		if (b1.currDir == 0 || b1.currDir == 3)
			graphic.depth = b1.graphic.depth-1;
		else
			graphic.depth = b1.graphic.depth+1;
		
		
	}
}

