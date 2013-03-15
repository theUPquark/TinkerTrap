using System;
using UnityEngine;

public class Bot3 : Player, Obstacle
{
	public Bot3 () : base(3)
	{
	}
	
	public Bot3 (double x, double y) : base(3,x,y)
	{
	}
	
	public override double getSpeed (double speed)
	{
		return (Math.Floor (speed*1.5));
	}
}
