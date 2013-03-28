using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
<<<<<<< HEAD
	public Bot2 ()
=======
	private bool hovering = false;
		
	public Bot2 () : base(2)
>>>>>>> 3e7b522caf0a1d5ed85940357c914e1132fb5824
	{
	}
	
	public Bot2 (double x, double y)
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
