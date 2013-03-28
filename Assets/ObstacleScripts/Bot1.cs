using System;
using UnityEngine;

public class Bot1 : Player, Obstacle
{
	public bool grabbing = false;
	public bool extendingArms = false;
	public Obstacle grabbed;
	
	public Bot1 ()
	{
		os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
	}
	
	public Bot1 (double x, double y)
	{
		os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
	}
	
	public override void setDir(int dir)
	{
		int startDir = currDir;
		if (!grabbing) {
			currDir = dir;
		}
		if (currDir != startDir)
		{
			if (Time.time-stopTime >= 1) {
				if (startDir == currDir-1 || startDir == currDir+3) {
					animPlay = true;
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnRtSt");
				} else if (startDir == currDir+1 || startDir == currDir-3) {
					animPlay = true;
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnLftSt");
				}
			} else {
				os.PlayLoop (this.GetType().Name + dirStr (currDir)+"_Move");
			}
		}
		if (!animPlay) {
			if (!moving) {
				moving = moveIntro = animPlay = true;
				os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_MvInt");
			} else if (moveIntro) {
				if (!os.isPlaying) {
					os.PlayLoop (this.GetType().Name + dirStr (currDir)+"_Move");
					moveIntro = false;
				}
			}
		}
	}
	
	public override void update()
	{
		if (moving) {
			if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)
				|| Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))) {
				moveIntro = animPlay = moving = false;
				os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
			} else {
				stopTime = Time.time;
			}
		}
		if (animPlay && !os.isPlaying) {
			animPlay = false;
		}
	}
	
	public override void primary (Tile a)
	{
		if (grabbing)
			Release ();
	}
	
	public override void primary(Tile a, Obstacle b)
	{
		if (!b.GetType ().IsSubclassOf (typeof(Player))) {
			if (!grabbing)
				Grab (b);
			else
				Release ();
		}	
	}
	
	public void Grab (Obstacle a) {
		grabbing = true;
		grabbed = a;
	}
	
	public void Release() {
		grabbing = false;
		grabbed = null;
	}
	
	public override bool inAction() {
		if (extendingArms || animPlay)
			return true;
		return false;
	}
}
