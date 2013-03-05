using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

	public Transform prefab;
	public int numberOfObjects;
	
	private static float tileW = 64;
	
	private int[,] map = new int[,] {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 8, 8, 8, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7}, {1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 7}, {1, 1, 1, 1, 1, 5, 1, 1, 3, 1, 1, 7}, {1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 7}, {1, 1, 1, 0, 1, 1, 1, 1, 2, 1, 1, 7}, {1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 7}, {1, 1, 1, 0, 4, 0, 0, 0, 0, 1, 1, 7}, {1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 7}, {1, 1, 1, 2, 1, 1, 1, 1, 5, 1, 1, 7}, {1, 1, 1, 0, 0, 4, 0, 0, 0, 1, 1, 7}, {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7}};
	private int[,] obsMap = new int[,] {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
	private int[,][] connectionMap = new int[,][]
	{
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {13,14}, new int[] {12,13}, new int[] {11,12}, new int[] {10,11}, new int[] {9,10}, new int[] {8,9}, new int[] {7,8}, new int[] {6,7}, new int[] {5,6}, new int[] {4,5}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {4}, new int[] {0}, new int[] {4}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
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
	private int[,][] locksMap =	new int[,][]
	{
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {3}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {3}, new int[] {0}, new int[] {3}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {1}, new int[] {0}, new int[] {0}, new int[] {2}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {1,2}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {1}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}},
		{new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}, new int[] {0}}
	};
																					
	private Dictionary<string, Tile> gameB = new Dictionary<string, Tile>();
	private Dictionary<int, List<Tile>> gameCons = new Dictionary<int, List<Tile>>();
	private Dictionary<int, List<Tile>> gameLocks = new Dictionary<int, List<Tile>>();
	private List<Obstacle> gameObs = new List<Obstacle>();
	
	private Obstacle player;
	
	void Start () {
		numberOfObjects = map.Length;
		
		int mapWidth = map.GetLength(0);
		int mapHeight = map.GetLength(1);

		OTSprite os;
		Vector2 pos;
		
		//loop to place tiles on stage
		for (int i = 0; i<mapHeight; ++i)
		{
			for (int j = 0; j<mapWidth; ++j)
			{	
				string squareName = "tile_"+i+"_"+j;
				
				gameB.Add(squareName, new Tile(map[j,i], i, j, OT.CreateObject("WorldTiles")));
					
				gameB[squareName].xiso = (-j+i)*tileW;
				gameB[squareName].yiso = (-j-i)*tileW/2F;
				
				pos = new Vector2 (gameB[squareName].xiso, gameB[squareName].yiso);
				os = gameB[squareName].gfx.GetComponent<OTSprite>();
				os.frameName = gameB[squareName].frameName;
				os.position = pos;
				
				if (gameB[squareName].walkable)
					os.depth = 1;
				else
					os.depth = -1;
				
				foreach (int k in connectionMap[j,i]) {
					List<Tile> tilelist;
					if (k != 0) {
						if (!gameCons.ContainsKey(k)) {
							tilelist = new List<Tile>();
							tilelist.Add(gameB[squareName]);
							gameCons.Add(k, tilelist);
						} else {
							tilelist = gameCons[k];
							tilelist.Add(gameB[squareName]);
						}
						gameB[squareName].addConnection(k, gameCons[k]);
					}
				}
				
				foreach (int k in locksMap[j,i]) {
					List<Tile> tilelist;
					if (k != 0) {
						if (!gameLocks.ContainsKey(k)) {
							tilelist = new List<Tile>();
							tilelist.Add(gameB[squareName]);
							gameLocks.Add(k, tilelist);
						} else {
							tilelist = gameLocks[k];
							tilelist.Add(gameB[squareName]);
						}
						gameB[squareName].addLock(k, gameLocks[k]);
					}
				}
				
				if (obsMap[j,i] != 0) {
					switch (obsMap[j,i]) {
						case 1:
							player = new Obstacle(1, i, j); //Creates and sets player on tile
							gameObs.Add (player);
							break;
						case 2:
							gameObs.Add (new Obstacle(2, i, j));
							break;
					}
				}
				
			}
		}
	}
	
	void OnGUI () {
		GUI.Box(new Rect(10,10,150,40), "Player tile: "+player.onTile());
	}
	
	void Update() {
		
		//Directional movement. Should this be limited to one direction at a time?
		
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
		
		// Scan for player interaction. This probably needs updating for different bot abilites.
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			interact();
		}
		
		// Do round 1 of Tile updates. Final 'act' method called in LateUpdate();
		foreach (Tile t in gameB.Values) {
			t.update();
		}
	}
	
	// Standard Unity Late Update, occurs after Update
	// Using this to redraw tile depths after positions are adjusted in the main Update
	// Also moves the LevelLoader object to remain centered on the player, which moves the child camera as well
	
	void LateUpdate() {
		OTSprite p = player.gfx.GetComponent<OTSprite>();
		Vector3 posUp = new Vector3(p.position.x, p.position.y, transform.localPosition.z);
		transform.localPosition = posUp;
		
		// Do round 2 of Tile updates. Initial 'update' method called in Update();
		foreach (Tile t in gameB.Values) {
			t.act(gameObs);
		}
		
		OTSprite ps = player.gfx.GetComponent<OTSprite>();
		OTSprite tos;
		// Since Orthello automatically adjusts z depth based on y position for objects on the same layer,
		// all we have to do is make sure walls in front of the player are on a higher layer and walls behind the player are on a lower layer.
		// This may need some tweaking for movable obstacles, which use the same base class as the player (Obstacle).
		foreach (var i in gameB) {
			if (i.Value.gfx != null) {
				tos = i.Value.gfx.GetComponent<OTSprite>();
				if (tos.position.y > ps.position.y+player.width/4 || i.Value.walkable)
					tos.depth = 2;
				else
					tos.depth = -1;
			}
		}
		foreach (Obstacle i in gameObs) {
			if (i.gfx != null && i != player) {
				tos = i.gfx.GetComponent<OTSprite>();
				if (tos.position.y > ps.position.y+player.width/4)
					tos.depth = 1;
				else
					tos.depth = -1;
			}
		}
	}
	
	public static float getTileW()
	{	return tileW;	}
	
	private void interact() {
		Tile atTile = null;
		string tarTile = "";
		if (player.currDir == 0) {
			tarTile = "tile_"+(int)(player.xtile)+"_"+(int)(player.ytile-1);
		}
		if (player.currDir == 1) {
			tarTile = "tile_"+(int)(player.xtile-1)+"_"+(int)(player.ytile);
		}
		if (player.currDir == 2) {
			tarTile = "tile_"+(int)(player.xtile)+"_"+(int)(player.ytile+1);
		}
		if (player.currDir == 3) {
			tarTile = "tile_"+(int)(player.xtile+1)+"_"+(int)(player.ytile);
		}
		
		/*if (atTile.lockGrou[ {0] != 0) {
			for (var i: int = 0; i < atTile.lockGroup.length; i++) {
				for (var j: int = 0; j < gameBLoc[ {String(atTile.lockGrou[ {i])].length; j++) {
					if (gameBLoc[ {String(atTile.lockGrou[ {i])]new int[] {j].locked)
						return;
				}
			}
		}*/
		
		atTile = gameB[tarTile];
		
		if (atTile != null)
			atTile.interact();
	}
	
	// getMyCorners is called to detect the player position and dimensions, checking if movement will carry the player into a new tile.
	// This data is used by the moveChar function to decide where to move the player.
	
	private void getMyCorners(Obstacle tob, double px, double py)
	{
		tob.downYPos = py+tob.width/2-1;
		tob.upYPos = py;
		tob.leftXPos = px;
		tob.rightXPos = px+tob.width/2-1;
		//find corner points
		tob.downY = Math.Floor(tob.downYPos/tileW);
		tob.upY = Math.Floor(tob.upYPos/tileW);
		tob.leftX = Math.Floor(tob.leftXPos/tileW);
		tob.rightX = Math.Floor(tob.rightXPos/tileW);
		//check if they are walls
		if (tob.upY >= 0 || tob.downY < map.GetLength(0) ||
			tob.rightX >= 0 || tob.leftX < map.GetLength(1)) {
			tob.upleft = gameB["tile_"+tob.leftX+"_"+tob.upY].walkable;
			tob.downleft = gameB["tile_"+tob.leftX+"_"+tob.downY].walkable;
			tob.upright = gameB["tile_"+tob.rightX+"_"+tob.upY].walkable;
			tob.downright = gameB["tile_"+tob.rightX+"_"+tob.downY].walkable;
		}
		if (tob.upY < 0)
			tob.upleft = tob.upright = false;
		if (tob.downY >= map.GetLength(0))
			tob.downleft = tob.downright = false;
		if (tob.rightX < 0)
			tob.upright = tob.downright = false;
		if (tob.leftX >= map.GetLength(1))
			tob.upleft = tob.downleft = false;
		
	}
	
	// Move char handles all Obstacle movement. Currently this only means the player, but it is designed to pass a final speed backwards.
	// This is so that Obstacles can chain movement and reduce speed depending on the number of stacked Obstacles being pushed.
	
	private double moveChar(Obstacle tob, double speed, int dirx, int diry)
	{
		
		double speedAdj = speed;
		
		//vert movement
		//changing y with speed and taking old x
		getMyCorners(tob, tob.posX, tob.posY+speed*diry);
		
		//if going up
		if (diry == -1)
		{
			if (tob.upleft && tob.upright)
			{
				foreach (Obstacle iob in gameObs) {
					if (iob != tob) {
						getMyCorners(iob, iob.posX, iob.posY);
						if ( tob.upYPos < iob.downYPos && tob.downYPos > iob.upYPos &&
							tob.leftXPos < iob.rightXPos && tob.rightXPos > iob.leftXPos) {
							Console.WriteLine (tob.type + ": thisdowny: " + tob.downYPos + " | thatupy:" + iob.upYPos);
							Console.WriteLine (tob.type + ": thisrightx: " + tob.rightXPos + " | thatleftx:" + iob.leftXPos);
							speedAdj = moveChar(iob, speed/2, dirx, diry);
						}
					}
				}
				tob.setY((float)(tob.posY+speedAdj*diry));
			}
			else
			{
				//hit the wall, place tob near the wall
				tob.setY(((float)((tob.ytile)*tileW)));
				speedAdj = 0;
			}
		}
		//if going down
		if (diry == 1)
		{
			if (tob.downleft && tob.downright)
			{
				foreach (Obstacle iob in gameObs) {
					if (iob != tob) {
						getMyCorners(iob, iob.posX, iob.posY);
						if ( tob.downYPos > iob.upYPos && tob.upYPos < iob.downYPos &&
							tob.leftXPos < iob.rightXPos && tob.rightXPos > iob.leftXPos) {
							Console.WriteLine (tob.type + ": thisdowny: " + tob.downYPos + " | thatupy:" + iob.upYPos);
							Console.WriteLine (tob.type + ": thisrightx: " + tob.rightXPos + " | thatleftx:" + iob.leftXPos);
							speedAdj = moveChar(iob, speed/2, dirx, diry);
						}
					}
				}
				tob.setY((float)(tob.posY+speedAdj*diry));
			}
			else
			{
				tob.setY(((float)((tob.ytile+1)*tileW-(tob.width/2))));
				speedAdj = 0;
			}
		}
		//horizontal movement
		//changing x with speed and taking old y
		getMyCorners(tob, tob.posX+speed*dirx, tob.posY);
		//if going left
		if (dirx == -1)
		{
			if (tob.downleft && tob.upleft)
			{
				foreach (Obstacle iob in gameObs) {
					if (iob != tob) {
						getMyCorners(iob, iob.posX, iob.posY);
						if ( tob.leftXPos < iob.rightXPos && tob.rightXPos > iob.leftXPos &&
							tob.upYPos < iob.downYPos && tob.downYPos > iob.upYPos) {
							Console.WriteLine (tob.type + ": thisdowny: " + tob.downYPos + " | thatupy:" + iob.upYPos);
							Console.WriteLine (tob.type + ": thisrightx: " + tob.rightXPos + " | thatleftx:" + iob.leftXPos);
							speedAdj = moveChar(iob, speed/2, dirx, diry);
						}
					}
				}
				tob.setX((float)(tob.posX+speedAdj*dirx));
			}
			else
			{
				tob.setX(((float)(tob.xtile*tileW)));
				speedAdj = 0;
			}
		}
		//if going right
		if (dirx == 1)
		{
			if (tob.upright && tob.downright)
			{
				foreach (Obstacle iob in gameObs) {
					if (iob != tob) {
						getMyCorners(iob, iob.posX, iob.posY);
						if ( tob.rightXPos > iob.leftXPos && tob.leftXPos < iob.rightXPos &&
							tob.upYPos < iob.downYPos && tob.downYPos > iob.upYPos) {
							Console.WriteLine (tob.type + ": thisdowny: " + tob.downYPos + " | thatupy:" + iob.upYPos);
							Console.WriteLine (tob.type + ": thisrightx: " + tob.rightXPos + " | thatleftx:" + iob.leftXPos);
							speedAdj = moveChar(iob, speed/2, dirx, diry);
						}
					}
				}
				tob.setX((float)(tob.posX+speedAdj*dirx));
			}
			else
			{
				tob.setX(((float)((tob.xtile+1)*tileW-(tob.width/2))));
				speedAdj = 0;
			}
		}
		
		return (speedAdj);
	}
}