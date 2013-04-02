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
	
	public override int width {
		get { return 70; }
	}
	
	public override void setDir(int dir)
	{
		int startDir = currDir;
		if (animDir == null)
			animDir = dir;
		else
			moveTry = true;
		if (!grabbing) {
			currDir = dir;
		}
		if (currDir != startDir)
			turning = true;
	}
	
	public override void update(bool input)
	{
		if (moving || moveIntro) {
			if (!input) {
				stopping = true;
			}
		}
		if (moveTry && input)
			stopping = false;
		if (!os.isPlaying || os.animationFrameset.Equals (this.GetType ().Name + dirStr (animDir)+"_MvStop") || os.animationFrameset.Equals (this.GetType ().Name + dirStr (animDir)+"_Idle")) {
			animPlay = false;
			if (!grabbing) {
				if (turning) {
					if (idle) {
						if (animDir == currDir-1 || animDir == currDir+3)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnRtSt");
						else if (animDir == currDir+1 || animDir == currDir-3)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnLftSt");
						else if (Math.Abs (animDir-currDir) == 2)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnRvSt");
					} else {
						if (animDir == currDir-1 || animDir == currDir+3)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnRtMv");
						else if (animDir == currDir+1 || animDir == currDir-3)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnLftMv");
						else if (Math.Abs (animDir-currDir) == 2)
							os.PlayOnce (this.GetType ().Name + dirStr (animDir)+"_TurnRvMv");
					}
					if (os.isPlaying)
						animDir = currDir;
					turning = false;
				} else if (stopping) {
					if (!os.isPlaying) {
						if (!playstop && !idle) {
							os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_MvStop");
							animDir = currDir;
							playstop = true;
						} else {
							os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
							animDir = currDir;
							stopping = false;
							idle = true;
							playstop = false;
						}
					}
				} else if (moveTry) {
					if (idle) {
						os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_MvInt");
						animDir = currDir;
						idle = false;
					} else {
						os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_Move");
						animDir = currDir;
						moving = true;
					}
					moveTry = false;
				}
			}
		}
	}
	
	public override void primary (Tile a)
	{
		Debug.Log ("Bot1 Empty Primary");
		if (grabbing)
			Release ();
	}
	
	public override void primary(Tile a, Obstacle b)
	{
		Debug.Log ("Bot1 Using Primary");
		
		if (!b.GetType ().IsSubclassOf (typeof(Player))) {
			if (grabbing) {
				Release ();
			} else {
				Grab (b);
			}
		}	
	}
	
	public void Grab (Obstacle a) {
		grabbing = true;
		grabbed = a;
		os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_Grab");
	}
	
	public void Release() {
		grabbing = false;
		grabbed = null;
		os.PlayOnceBackward (this.GetType ().Name + dirStr (currDir)+"_Grab");
	}
	
	public override bool inAction() {
		if (extendingArms || animPlay)
			return true;
		return false;
	}
}
