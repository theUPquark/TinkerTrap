using System;
using UnityEngine;
using System.Collections.Generic;

public class Box : ObstacleClass, Obstacle
{
	public Box (int a) : base(a) {}
	
	public Box (int a, double x, double y) : base(a,x,y) {}
	
	public override int width {
		get { return 70; }
	}
	
	public override double getSpeed (double speed, Obstacle source)
	{
		double adjSpeed = base.getSpeed (speed, source); //Should return 1/2 speed
		if (source.GetType() == typeof(Bot2)) //Bot2 can't push boxes.
			return 0.0;
		if (source.GetType() == typeof(Bot3))
			//Since the default speed is already halved, greenbot only pushes at 1/4 speed.
			return Math.Floor (adjSpeed/2);
		return adjSpeed;
	}
}

