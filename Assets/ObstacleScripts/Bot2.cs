using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
	public Bot2 ()
	{
	}
	
	public Bot2 (double x, double y)
	{
	}
	
	public override void primary(Tile a)
	{
		if (a.GetType () == typeof(Generator))
			a.interact (this);
	}
}
