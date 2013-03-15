using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
	public Bot2 () : base(2)
	{
	}
	
	public Bot2 (double x, double y) : base(2,x,y)
	{
	}
	
	public override void primary(Tile a)
	{
		if (a.GetType () == typeof(Generator))
			a.interact (this);
	}
}
