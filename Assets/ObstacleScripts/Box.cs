using System;
using UnityEngine;

public class Box : ObstacleClass, Obstacle
{
	public Box (int a) : base(a)
	{
		os.frameName = "Box0";
	}
	
	public Box (int a, double x, double y) : base(a,x,y)
	{
		os.frameName = "Box0";
	}
	
	public override int width {
		get { return 70; }
	}
}

