using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

	public Transform prefab;
	public int numberOfObjects;
	
	private float tileW = 32;
	
	private int[,] map = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 8, 8, 8, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 7}, {1, 1, 1, 1, 1, 5, 1, 1, 3, 1, 1, 7}, {1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 7}, {1, 1, 1, 0, 1, 1, 1, 1, 2, 1, 1, 7}, {1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 7}, {1, 1, 1, 0, 4, 0, 0, 0, 0, 1, 1, 7}, {1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 7}, {1, 1, 1, 2, 1, 1, 1, 1, 5, 1, 1, 7}, {1, 1, 1, 0, 0, 4, 0, 0, 0, 1, 1, 7}, {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7}};
	private int[,] obsMap = new int[,] {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
	private int[,][] connectionMap = new int[,][]
	{
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {13}, new int[] {12,13}, new int[] {11,12}, new int[] {10,11}, new int[] {9,10}, new int[] {8,9}, new int[] {7,8}, new int[] {6,7}, new int[] {5,6}, new int[] {4,5}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {4}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {2}, new int[] {0}, new int[] {0}, new int[] {1}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {3}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {1}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {2}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {3}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {1}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}}
	};
	private Dictionary<string, AssemblyCSharp.Tile> gameB = new Dictionary<string, AssemblyCSharp.Tile>();
	
	private AssemblyCSharp.Obstacle player;
	
	void Start () {
		numberOfObjects = map.Length;
		
		int mapWidth = map.GetLength(0);
		int mapHeight = map.GetLength(1);
		player = new AssemblyCSharp.Obstacle(0,OT.CreateObject("Bot1"));
		OTSprite os;
		Vector2 pos;
		
		//loop to place tiles on stage
		for (int i = 0; i<mapHeight; ++i)
		{
			for (int j = 0; j<mapWidth; ++j)
			{	
				string squareName = "tile_"+j+"_"+i;
				
				gameB.Add(squareName, new AssemblyCSharp.Tile(map[j,i], j, i, OT.CreateObject("WorldTiles")));
					
				gameB[squareName].xiso = (-j+i)*tileW;
				gameB[squareName].yiso = (-j-i)*tileW/2F;
				
				pos = new Vector2 (gameB[squareName].xiso, gameB[squareName].yiso);
				os = gameB[squareName].gfx.GetComponent<OTSprite>();
				os.frameIndex = gameB[squareName].type;
				switch (os.frameIndex) {
				case 1:
					os.frameName = "Wall0";
					break;
				}
				os.position = pos;
				
				if (gameB[squareName].walkable)
					os.depth = 1;
				else
					os.depth = -1;
				
				/*for (k = 0; k < connectionMap[i][j].length; k++) {
				    if (connectionMap[i][j][0] != 0) {
                        if (gameBCon[String(connectionMap[i][j][k])] == null) {
                            gameBCon[String(connectionMap[i][j][k])] = new Array();
                        }
                        gameBCon[String(connectionMap[i][j][k])].push(gameB[squareName]);
					}
                    gameB[squareName].setConnection(connectionMap[i][j][k]);
				}
				
				for (k = 0; k < locksMap[i][j].length; k++) {
				    if (locksMap[i][j][0] != 0) {
                        if (gameBLock[String(locksMap[i][j][k])] == null) {
                            gameBLock[String(locksMap[i][j][k])] = new Array();
                        }
                        gameBLock[String(locksMap[i][j][k])].push(gameB[squareName]);
					}
                    gameB[squareName].setLock(locksMap[i][j][k]);
				}*/
				
				if (obsMap[j,i] != 0) {
					switch (obsMap[j,i]) {
						case 1:
							player.xtile = j;
							player.ytile = i;
							break;
						/*case 2:
							gameBObs.push(new obstacle(2));
							gameBObs[gameBObs.length-1].xtile = i;
							gameBObs[gameBObs.length-1].ytile = j;
							break;*/
					}
				}
				
			}
		}
		//add the playeracter mc and insert at correct layer
		os = player.gfx.GetComponent<OTSprite>();
		
		player.posX = -player.xtile*tileW;
		player.posY = -player.ytile*tileW;
		//calculate position in isometric view
		player.xiso = (float)(player.posX-player.posY);
		player.yiso = (float)((player.posX+player.posY)/2);
		
		pos = new Vector2(player.xiso, player.yiso);
		os.position = pos;
		//calculate the tile where players center is
		player.xtile = -Math.Floor((player.posX)/tileW);
		player.ytile = -Math.Floor((player.posY)/tileW);
		
		foreach (var i in gameB) {
			OTSprite tos = i.Value.gfx.GetComponent<OTSprite>();
			if (tos.position.y > os.position.y || i.Value.walkable)
				tos.depth = 1;
			else
				tos.depth = -1;
		}
	}
	
	void Update() {
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
		{
			moveChar(player, 5.0, -1, 0);
			player.setDir(3);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
		{
			moveChar(player, 5.0, 1, 0);
			player.setDir(1);
		}

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
		{
			moveChar(player, 5.0, 0, -1);
			player.setDir(0);
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
		{
			moveChar(player, 5.0, 0, 1);
			player.setDir(2);
		}
	}
	
	void LateUpdate() {
		OTSprite p = player.gfx.GetComponent<OTSprite>();
		Vector3 posUp = new Vector3(p.position.x, p.position.y, transform.localPosition.z);
		transform.localPosition = posUp;
		
		
		OTSprite os = player.gfx.GetComponent<OTSprite>();
		OTSprite tos;
		foreach (var i in gameB) {
			if (i.Value.gfx != null) {
				tos = i.Value.gfx.GetComponent<OTSprite>();
				if (tos.position.y > os.position.y || i.Value.walkable)
					tos.depth = 1;
				else
					tos.depth = -1;
			}
		}
	}
	
	private void getMyCorners(AssemblyCSharp.Obstacle tob, double px, double py)
	{
		tob.downYPos = py;
		tob.upYPos = py+32-1;
		tob.leftXPos = px;
		tob.rightXPos = px+32-1;
		//find corner points
		tob.downY = -Math.Floor(tob.downYPos/tileW);
		tob.upY = -Math.Floor(tob.upYPos/tileW);
		tob.leftX = -Math.Floor(tob.leftXPos/tileW);
		tob.rightX = -Math.Floor(tob.rightXPos/tileW);
		//check if they are walls
		Console.WriteLine("{0}, {1}, {2}, {3}", tob.downY, tob.upY, tob.rightX, tob.leftX);
		if (tob.downY >= 0 || tob.upY < map.GetLength(0) ||
			tob.leftX >= 0 || tob.rightX < map.GetLength(1)) {
			tob.upleft = gameB["tile_"+tob.leftX+"_"+tob.upY].walkable;
			tob.downleft = gameB["tile_"+tob.leftX+"_"+tob.downY].walkable;
			tob.upright = gameB["tile_"+tob.rightX+"_"+tob.upY].walkable;
			tob.downright = gameB["tile_"+tob.rightX+"_"+tob.downY].walkable;
		}
		if (tob.downY < 0)
			tob.downleft = tob.downright = false;
		if (tob.upY >= map.GetLength(0))
			tob.upleft = tob.upright = false;
		if (tob.leftX < 0)
			tob.upleft = tob.downleft = false;
		if (tob.rightX >= map.GetLength(1))
			tob.upright = tob.downright = false;
		
	}
	
	private double moveChar(AssemblyCSharp.Obstacle tob, double speed, int dirx, int diry)
	{
		
		double speedAdj = speed;
		
		getMyCorners(tob, tob.posX, tob.posY-speed*diry);
		
		
		if (diry == -1)
		{
			if (tob.upleft && tob.upright)
			{
				/*for (int i = 0; i < gameBObs.Length; i++) {
					if (gameBObs[i] != tob) {
						getMyCorners(gameBObs[i], gameBObs[i].posX, gameBObs[i].posY);
						if (Math.abs((gameBObs[i].upYPos+gameBObs[i].downYPos)/2 -
							 (tob.upYPos+tob.downYPos-1)/2) < (gameBObs[i].width+tob.width)/4 &&
							tob.xtile == gameBObs[i].xtile) {
							speedAdj = moveChar(gameBObs[i], speed/2, dirx, diry);
						}
					}
				}*/
				tob.setY((float)(tob.posY-speedAdj*diry));
			}
			else
			{
				//hit the wall, place tob near the wall
				tob.setY(-((float)((tob.ytile-1)*tileW+1)));
				speedAdj = 0;
			}
		}
		//if going down
		if (diry == 1)
		{
			if (tob.downleft && tob.downright)
			{
				/*for (i = 0; i < gameBObs.length; i++) {
					if (gameBObs[i] != tob) {
						getMyCorners(gameBObs[i], gameBObs[i].posX, gameBObs[i].posY);
						if (Math.abs((gameBObs[i].upYPos+gameBObs[i].downYPos)/2 -
							 (tob.upYPos+tob.downYPos-1)/2) <  (gameBObs[i].width+tob.width)/4 &&
							tob.xtile == gameBObs[i].xtile) {
							speedAdj = moveChar(gameBObs[i], speed/2, dirx, diry);
						}
					}
				}*/
				tob.setY((float)(tob.posY-speedAdj*diry));
			}
			else
			{
				tob.setY(-((float)((tob.ytile)*tileW)));
				speedAdj = 0;
			}
		}
		//horisontal movement
		//changing x with speed and taking old y
		getMyCorners(tob, tob.posX+speed*dirx, tob.posY);
		//if going left
		if (dirx == -1)
		{
			if (tob.downleft && tob.upleft)
			{
				/*for (i = 0; i < gameBObs.length; i++) {
					if (gameBObs[i] != tob) {
						getMyCorners(gameBObs[i], gameBObs[i].posX, gameBObs[i].posY);
						if (Math.abs((gameBObs[i].leftXPos+gameBObs[i].rightXPos)/2 -
							 (tob.leftXPos+tob.rightXPos-1)/2) <  (gameBObs[i].width+tob.width)/4 &&
							tob.ytile == gameBObs[i].ytile) {
							speedAdj = moveChar(gameBObs[i], speed/2, dirx, diry);
						}
					}
				}*/
				tob.setX((float)(tob.posX+speedAdj*dirx));
			}
			else
			{
				tob.setX(-((float)((tob.xtile)*tileW-1)));
				speedAdj = 0;
			}
		}
		//if going right
		if (dirx == 1)
		{
			if (tob.upright && tob.downright)
			{
				/*for (i = 0; i < gameBObs.length; i++) {
					if (gameBObs[i] != tob) {
						getMyCorners(gameBObs[i], gameBObs[i].posX, gameBObs[i].posY);
						if (Math.abs((gameBObs[i].leftXPos+gameBObs[i].rightXPos)/2 -
							 (tob.leftXPos+tob.rightXPos-1)/2) <  (gameBObs[i].width+tob.width)/4 &&
							tob.ytile == gameBObs[i].ytile) {
							speedAdj = moveChar(gameBObs[i], speed/2, dirx, diry);
						}
					}
				}*/
				tob.setX((float)(tob.posX+speedAdj*dirx));
			}
			else
			{
				tob.setX(-((float)((tob.xtile-1)*tileW+1)));
				speedAdj = 0;
			}
		}
		
		return (speedAdj);
	}
}