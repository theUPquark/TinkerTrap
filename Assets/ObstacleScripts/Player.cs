using System;
using UnityEngine;

public abstract class Player : ObstacleClass, Obstacle
{
	public int currDir;
	protected bool animPlay = false;
	protected bool moveIntro = false;
	protected bool moving = false;
	protected bool stopping = false;
	protected bool playstop = false;
	protected float stopTime = Time.time;
	public int level = 0;
	
	public Player() : base(1)
	{
		setDir (0);
	}
	
	public Player(double x, double y) : base(1,x,y)
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
	
	protected string dirStr(int a) {
		switch (a) {
		default:
			return "UpRt";
			break;
		case 1:
			return "DnRt";
			break;
		case 2:
			return "DnLft";
			break;
		case 3:
			return "UpLft";
			break;
		}
	}
	
	public virtual void primary(Tile a) {}
	public virtual void primary(Tile a, Obstacle b)
	{
		this.primary(a);
	}
	// ADD: Method to determine coordinates, currently in GameManager.moveChar
	// ADD: Method to determine coordinate area to perform a turn.
	
	public virtual void update(bool input) {}
}

