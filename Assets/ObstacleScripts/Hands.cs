using System;
using System.Collections.Generic;
using UnityEngine;

public class Hands : ObstacleClass, Obstacle
{
	Bot1 b1;
	
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
		
		
	}
}

