using UnityEngine;
using System;
using System.Collections.Generic;


public class Obstacle
{
	
	
	public double xtile, ytile, depthshift, depth;
	public double speed, posX, posY;
	public double downY, upY, leftX, rightX;
	public double downYPos, upYPos, leftXPos, rightXPos;
	public float xiso, yiso;
	public bool upleft, downleft, upright, downright;
	public int type, width, currDir;
	public GameObject gfx;
	
	public Obstacle (int a)
	{
		type = a;
		switch(type) {
		case 1:
			setDir(3);
			gfx = OT.CreateObject("Bot1");
			width = 63;
			break;
		case 2:
			gfx = OT.CreateObject("Box1");
			width = 70;
			break;
		}
	}
	
	public Obstacle (int a, double x, double y)
	{
		type = a;
		switch(type) {
		case 1:
			setDir(3);
			gfx = OT.CreateObject("Bot1");
			width = 63;
			break;
		case 2:
			gfx = OT.CreateObject("Box1");
			width = 70;
			break;
		}
		
		xtile = x;
		ytile = y;
		
		posX = xtile * LevelLoader.getTileW ();
		posY = ytile * LevelLoader.getTileW ();
		setPos();
	}
	
	// Returns the name of the Tile currently occupied by the Obstacle
	public string onTile()	{
		string tileName = "tile_"+xtile+"_"+ytile;
		return tileName;
	}

	public void setX (float x)
	{
		posX = (double)x;
		setPos();
	}

	public void setY (float y)
	{
		posY = (double)y;
		setPos();
	}
	
	private void setPos() 
	{
		OTSprite os = gfx.GetComponent<OTSprite>();
		//calculate position in isometric view
		xiso = (float)(posX-posY);
		yiso = (float)((-posY-posX)/2);
		//update tob position
		Vector2 pos = new Vector2(xiso, yiso);
		os.position = pos;
		//calculate the tile where tobs center is
		xtile = Math.Floor((posX/LevelLoader.getTileW ()));
		ytile = Math.Floor((posY/LevelLoader.getTileW ()));
	}
	
	public void setDir(int dir)
	{
		currDir = dir;
		if (gfx != null) {
			OTSprite os = gfx.GetComponent<OTSprite>();
			switch (currDir) {
			case 0:
				os.frameName = "Bot1UpRt";
				break;
			case 3:
				os.frameName = "Bot1UpLft";
				break;
			case 2:
				os.frameName = "Bot1DnLft";
				break;
			case 1:
				os.frameName = "Bot1DnRt";
				break;
			}
		}
	}
	
	public void setDepth(Dictionary<string, Tile> tileSheet)
	{
		OTSprite os = this.gfx.GetComponent<OTSprite>();
		
		foreach (var i in tileSheet) {
		OTSprite tos = i.Value.gfx.GetComponent<OTSprite>();
		if (tos.position.y > os.position.y || i.Value.walkable)
			tos.depth = 1;
		else
			tos.depth = -1;
		}
	}
}

