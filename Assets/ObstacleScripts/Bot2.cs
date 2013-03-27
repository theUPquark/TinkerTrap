using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
	private bool hovering = false;
		
	public Bot2 () : base(2)
	{
	}
	
	public Bot2 (double x, double y) : base(2,x,y)
	{
	}
	
	public void primary(Tile a, Tile b)
	{
		if (level >= 2) {
			if (b != null) {
				
			}
		}
	}
	
	public override bool inAction()
	{
		//Hover Conditions
		if (!hovering /*&&  Things*/){
			return false;
		} else 
			return true;
	}
}
