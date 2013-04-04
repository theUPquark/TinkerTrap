using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
	private bool hovering = false;
	private string hoverTargetL = "";
	private string hoverTargetR = "";
		
	public Bot2 ()
	{
		os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
	}

	public Bot2 (double x, double y)
	{
		os.PlayLoop (this.GetType ().Name + dirStr (currDir)+"_Idle");
	}
	
	public override int width {
		get { return 85; }
	}
	
	public override void setDir(int dir)
	{
		int startDir = currDir;
		if (animDir == null)
			animDir = dir;
		else
			moveTry = true;
		currDir = dir;
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
	
	public void primary(Tile l1, Tile l2, Tile r1, Tile r2)
	{
		if (level >= 1 && !hovering) {
			if ( ( (l1.GetType().IsSubclassOf(typeof(Floor)) || l1.GetType() == typeof(Floor)) && l2.GetType() != typeof(Pit) && l2.walkable() ) 
				&& ( (r1.GetType().IsSubclassOf(typeof(Floor)) || r1.GetType() == typeof(Floor)) && r2.GetType() != typeof(Pit) && r2.walkable() ) ) {
				hoverTargetL = l2.myName();
				hoverTargetR = r2.myName();
				vertLift = 1;
				hovering = true;
			}
		}
	}
	
	public override bool inAction()
	{
		//Hover Conditions
		if (hovering){
			if ( (currDir == 1  || currDir == 2 ) && !onTile().Equals(hoverTargetL))
				return true;
			else if ( (currDir == 0  || currDir == 3 ) && !onTileBotR().Equals(hoverTargetR))
				return true;
			else {
				vertLift = 0;
				return hovering = false;
			}
		} else {
			vertLift = 0;
			return hovering = false;
		}
		
	}
	
	public override void endAction() {
		if (hovering) {
			hovering = false;
			vertLift = 0;
		}
	}
		
	public bool IsHovering() {return hovering;}
}
