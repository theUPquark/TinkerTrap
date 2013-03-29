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
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnRtSt");
				} else if (startDir == currDir+1 || startDir == currDir-3) {
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnLftSt");
				} else if (Math.Abs (startDir-currDir) == 2) {
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnRvSt");
				}
			} else {
				if (startDir == currDir-1 || startDir == currDir+3) {
					moveIntro = true;
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnRtMv");
				} else if (startDir == currDir+1 || startDir == currDir-3) {
					moveIntro = true;
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnLftMv");
				} else if (Math.Abs (startDir-currDir) == 2) {
					moveIntro = true;
					os.PlayOnce (this.GetType ().Name + dirStr (startDir)+"_TurnRvMv");
				}
			}
		}
		if (!animPlay) {
			if (!moving) {
				if (!moveIntro) {
					moveIntro = true;
					os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_MvInt");
				} else {
					if (!os.isPlaying) {
						os.PlayLoop (this.GetType().Name + dirStr (currDir)+"_Move");
						moveIntro = false;
						moving = true;
					}
				}
			} 
		}
		stopping = playstop = false;
	}
	
	public override void update(bool input)
	{
		if (moving || moveIntro) {
			if (!input) {
				stopping = true;
				moveIntro = false;
				if (Time.time - stopTime >= 1)
					moving = false;
			} else {
				stopTime = Time.time;
			}
		}
		if (stopping) {
			if (!os.isPlaying || os.animationFrameset.Equals (this.GetType ().Name + dirStr (currDir)+"_Move")) {
				if (!playstop) {
					os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_MvStop");
					playstop = true;
				} else {
					os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
					stopping = false;
					playstop = false;
				}
			}
		}
		if (!os.isPlaying)
			animPlay = false;
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
