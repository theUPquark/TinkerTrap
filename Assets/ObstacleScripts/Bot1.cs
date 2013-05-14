using System;
using System.Collections.Generic;
using UnityEngine;

public class Bot1 : Player, Obstacle
{
	public bool grabbing = false;
	public bool extendingArms = false;
	public bool retractArms = false;
	public bool startExtArms = false;
	public int extendDist = 0;
	public const int EXTEND_MAX = 192;
	public const int STEP_SIZE = 10;
	public Hands hands;
	public Obstacle grabbed;
	public AudioClip audioGrab;
	
	public Bot1 ()
	{
		hands = new Hands(this);
		
		audioGrab = Resources.Load ("clank") as AudioClip;
		gfx.AddComponent<AudioSource>().clip = audioGrab;
	}
	
	public Bot1 (double x, double y)
	{
//		arm.Add(new BotArm(x,y));
//		arm.Add(new BotArm(x,y));
			
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
		if (!grabbing && !startExtArms && !extendingArms && !retractArms) {
			base.update (input);
		}
		if (startExtArms && !os.isPlaying) {
			Debug.Log ("Show Hands");
			startExtArms = false;
			extendingArms = true;
			
//			hands.PlaceHands();
			hands.os.visible = true;
			hands.os.PlayOnce("Hands" + dirStr (currDir));
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
			if (grabbing) {
				Release ();
			} else {
				Grab (b);
			}
		}	
	}
	
	public void secondary () {
		if (level > 0){
//			hands.os.visible = true;
			hands.setX (posX);
			hands.setY (posY);
			
			os.PlayOnce(this.GetType().Name + dirStr(currDir)+"_Ext");
//			hands.os.PlayOnce("Hands" + dirStr (currDir));
			
//			extendingArms = true;
			startExtArms = true;
			Debug.Log("extendingArms TRUE");
		}
	}
	
	public void Grab (Obstacle a) {
		if (!grabbing) {
			gfx.GetComponent<AudioSource>().Play ();
			grabbing = true;
			grabbed = a;
			os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_Grab");
		}
	}
	
	public void Release() {
		grabbing = false;
		grabbed = null;
		os.PlayOnceBackward (this.GetType ().Name + dirStr (currDir)+"_Grab");
		if (extendingArms){
			extendingArms = false;
			retractArms = true;
		}
	}
	
	public override bool inAction() {
		if (extendingArms || retractArms/* || animPlay*/)
			return true;
		else 
			return false;
	}
	
	public override void endAction() {
		if (extendingArms){
			extendingArms = false;
			retractArms = true;
		}
//		retractArms = false;
	}
	
	public int ExtendArmsStep() {
		if (extendingArms){
			extendDist += STEP_SIZE;
			if (extendDist > EXTEND_MAX) {
				extendingArms = false;
				retractArms = true;
			}
		}
		if (retractArms){
			extendDist -= STEP_SIZE;
			if (extendDist < 0) {
				retractArms = false;
				hands.os.visible = false;
			}
		}
//		hands.setX(leftXPos);
//		hands.setY(upYPos);
		
		return extendDist;
	}
	
	public bool ExtendArmsAction (List<Obstacle> gObs) {			// Return true if there is a object to move? Could just check grabbed.
		if (upleft || upright || downleft || downright){	//remove this when arm objects in place
			return extendingArms = false;
		}
		if (extendingArms && !grabbing) {
			foreach (Obstacle o in gObs) {
				if (o != this) {
//					getMyCorners(o, o.posX, o.posY);
					if (upYPos < o.downYPos && upYPos > o.upYPos && leftXPos < o.rightXPos && leftXPos > o.leftXPos)
						Grab(o);
					if (downYPos < o.downYPos && downYPos > o.upYPos && leftXPos < o.rightXPos && leftXPos > o.leftXPos)
						Grab(o);
					if (upYPos < o.downYPos && upYPos > o.upYPos && rightXPos < o.rightXPos && rightXPos > o.leftXPos)
						Grab(o);
					if (downYPos < o.downYPos && downYPos > o.upYPos && rightXPos < o.rightXPos && rightXPos > o.leftXPos)
						Grab(o);
				}
			}
			if (grabbing){
				extendingArms = false;
				return true;
			}
		} else if (extendingArms && grabbing) {
			foreach (Obstacle o in gObs) {
				if (o != this && o != grabbed) {
//					getMyCorners(o, o.posX, o.posY);
					if (grabbed.upYPos < o.downYPos && grabbed.upYPos > o.upYPos && grabbed.leftXPos < o.rightXPos && grabbed.leftXPos > o.leftXPos)
						return extendingArms = false;
					if (grabbed.downYPos < o.downYPos && grabbed.downYPos > o.upYPos && grabbed.leftXPos < o.rightXPos && grabbed.leftXPos > o.leftXPos)
						return extendingArms = false;
					if (grabbed.upYPos < o.downYPos && grabbed.upYPos > o.upYPos && grabbed.rightXPos < o.rightXPos && grabbed.rightXPos > o.leftXPos)
						return extendingArms = false;
					if (grabbed.downYPos < o.downYPos && grabbed.downYPos > o.upYPos && grabbed.rightXPos < o.rightXPos && grabbed.rightXPos > o.leftXPos)
						return extendingArms = false;
				}
			}
			return true;
		}
		
		return false;
	}
}
