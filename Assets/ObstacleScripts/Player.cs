using System;
using UnityEngine;

public abstract class Player : ObstacleClass, Obstacle
{
	public int currDir;
	public int level = 0;
	
	public Player(int a) : base(a)
	{
		setDir (0);
	}
	
	public Player(int a, double x, double y) : base(a,x,y)
	{
		setDir (0);
	}
	
	public override int width {
		get { return 63; }
	}
	
	public virtual void setDir(int dir)
	{
		currDir = dir;
		if (gfx != null) {
			switch (currDir) {
			case 0:
				os.PlayLoop ("Bot"+type+"UpRt_Idle");
				break;
			case 3:
				os.PlayLoop ("Bot"+type+"UpLft_Idle");
				break;
			case 2:
				os.PlayLoop ("Bot"+type+"DnLft_Idle");
				break;
			case 1:
				os.PlayLoop ("Bot"+type+"DnRt_Idle");
				break;
			}
		}
	}
	
	public virtual void primary(Tile a) {}
	public virtual void primary(Tile a, Obstacle b)
	{
		this.primary(a);
	}
	// ADD: Method to determine coordinates, currently in GameManager.moveChar
	// ADD: Method to determine coordinate area to perform a turn.
}

