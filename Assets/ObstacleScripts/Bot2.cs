using System;
using UnityEngine;

public class Bot2 : Player, Obstacle
{
	private bool hovering = false;
	private string hoverTargetL = "";
	private string hoverTargetR = "";
	
	public AudioSource audioHover;
	public AudioSource idleHover;
		
	public Bot2 () {
		audioHover = gfx.AddComponent<AudioSource>();
		idleHover = gfx.AddComponent<AudioSource>();
		audioHover.clip = Resources.Load ("hover") as AudioClip;
		audioHover.volume = 0.2f;
		//idleHover.clip = Resources.Load ("electric_hover") as AudioClip;
	}

	public Bot2 (double x, double y) {}
	
	public override int width {
		get { return 85; }
	}
	
	public override void update(bool input) {
		if (!hovering) {
			base.update (input);
		}
	}
	
	public void primary(Tile l1, Tile l2, Tile r1, Tile r2)
	{
		if (level >= 1 && !hovering) {
			if ( ( (l1.GetType().IsSubclassOf(typeof(Floor)) || l1.GetType() == typeof(Floor)) && l2.GetType() != typeof(Pit) && l2.walkable() ) 
				&& ( (r1.GetType().IsSubclassOf(typeof(Floor)) || r1.GetType() == typeof(Floor)) && r2.GetType() != typeof(Pit) && r2.walkable() ) ) {
				hoverTargetL = l2.myName();
				hoverTargetR = r2.myName();
				vertLift = 20;
				hovering = true;
				os.PlayOnce (this.GetType ().Name + dirStr (currDir)+"_Hover");
			}
		}
	}
	
	public override bool inAction()
	{
		//Hover Conditions
		if (hovering){
			if (!audioHover.isPlaying) {
				idleHover.Stop ();
				audioHover.Play();
			}
			if ( (currDir == 1  || currDir == 2 ) && !onTile().Equals(hoverTargetL))
				return true;
			else if ( (currDir == 0  || currDir == 3 ) && !onTileBotR().Equals(hoverTargetR))
				return true;
			else {
				vertLift = 0;
				return hovering = false;
			}
		} else {
			if (audioHover.isPlaying)
				audioHover.Stop();
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
