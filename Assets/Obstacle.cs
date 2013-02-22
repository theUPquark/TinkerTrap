using UnityEngine;
using System;

namespace AssemblyCSharp
{
	public class Obstacle
	{
		
		
		public double xtile, ytile, depthshift, depth;
		public double speed, posX, posY;
		public double downY, upY, leftX, rightX;
		public double downYPos, upYPos, leftXPos, rightXPos;
		public float xiso, yiso;
		public bool upleft, downleft, upright, downright;
		public int type, currDir;
		public GameObject gfx;
		
		public Obstacle (int a, GameObject b)
		{
			currDir = 3;
			type = a;
			gfx = b;
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
			yiso = (float)((posX+posY)/2);
			//update tob position
			Vector2 pos = new Vector2(xiso, yiso);
			os.position = pos;
			//face the direction
			//tob.setFacing(dirx+diry*2+3);
			//calculate the tile where tobs center is
			xtile = -Math.Floor((posX/(os.size.x/2)));
			ytile = -Math.Floor((posY/(os.size.x/2)));
		}
	}
}

