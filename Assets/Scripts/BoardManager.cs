using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // Need to specify because there's an overlap between System and UnityEngine

//namespace Completed;

public class BoardManager : MonoBehaviour {
        
    public GameObject testWall;
    public GameObject yoghurt;
    public LayerMask blockingLayer;
    public bool innerWalls = false;


    void Awake()
    {

    }
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

            public Count (int min, int max)
            {
                minimum = min;
                maximum = max;
            }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            yoghurtBehaviour();
        }

    }

    public class TileData
    {
        public bool hasTile = false;
        int index {get; set;}
        public int x { get; set; }
        public int y { get; set; }
        bool beenChecked = false;
        bool monasteryWall = false;
        public BoxCollider2D boxCollider;
        public GameObject monasteryWallObject = null;
        public string description;
        public int yoghurtLevel = 0;
        public GameObject yoghurtOnTile;
        public bool grownThisTurn = false;


        public bool hasItems = false;
        // neighbouring tiles as integers
        // 0 1 2 
        // 3 c 4
        // 5 6 7

        public int[] nbTiles = new int[8];

        public TileData(int counter, int xInit, int yInit)
        {
            index = counter;
            x = xInit;
            y = yInit;
        }

        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return x;
        }
        public int getIndex()
        {
            return index;
        }

        private bool isMonasteryTile = false;
        public bool isMonastery()
        {
            return isMonasteryTile;
        }
        
        public void setIsMonastery(bool t)
        {
            isMonasteryTile = t;
        }

        public void setChecked(bool t)
        {
            beenChecked = t;
        }

        public bool isChecked()
        {
            return beenChecked;
        }

        public void setMonasteryWall(bool t)
        {
            monasteryWall = t;
        }

        public bool isMonasteryWall()
        {
            return monasteryWall;
        }

        public void yoghurtGrow(List<TileData> tM, GameObject yoghurt)
        {
            if(yoghurtOnTile != null)
            {
                // Get randomised cardinal directions
                int r = Random.Range(1, 5);
                if (r == 2)
                    r = 6;
               
                int targetTile = nbTiles[r];

                // Catch cases where tile doesn't exist
                if (targetTile != -1)
                {
                    // Make sure yoghurt doesn't exist on tile already and isn't growing on a wall
                    if (tM[targetTile].yoghurtOnTile == null && !tM[targetTile].isMonasteryWall())
                    {
                        // Make sure can't grow more than once a turn
                        if (!grownThisTurn)
                        {
                            // Different growth processes

                            tM[targetTile].yoghurtOnTile = Instantiate(yoghurt, new Vector3(tM[targetTile].x, tM[targetTile].y, 0), Quaternion.identity) as GameObject;
                        
                            
                            
                            
                            tM[targetTile].grownThisTurn = true;
                        }
                    }
                }
            }
        }


        // helper method
        public void showNeighbours(List<TileData> tM)
        {
            // neighbouring tiles as integers //

            ///////////
            // 0 1 2 //
            // 3 c 4 //
            // 5 6 7 //
            ///////////
            Debug.Log("To my up left is a tile indexed at: " + nbTiles[0]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[0]].x + ", " + tM[nbTiles[0]].y);
            Debug.Log("Above me is a tile indexed at: " + nbTiles[1]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[1]].x + ", " + tM[nbTiles[1]].y);
            Debug.Log("To my up right is a tile indexed at: " + nbTiles[2]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[2]].x + ", " + tM[nbTiles[2]].y);
            Debug.Log("To my left is a tile indexed at: " + nbTiles[3]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[3]].x + ", " + tM[nbTiles[3]].y);
            Debug.Log("To my right is a tile indexed at: " + nbTiles[4]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[4]].x + ", " + tM[nbTiles[4]].y);
            Debug.Log("To my bottom left is a tile indexed at: " + nbTiles[5]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[5]].x + ", " + tM[nbTiles[5]].y);
            Debug.Log("Below me is a tile indexed at: " + nbTiles[6]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[6]].x + ", " + tM[nbTiles[6]].y);
            Debug.Log("To my bottom right is a tile indexed at: " + nbTiles[7]);
            Debug.Log("The coordinates for that tile are: " + tM[nbTiles[7]].x + ", " + tM[nbTiles[7]].y);    
        }

    }

    public int columns = 8; 
    public int rows = 8;
    public List<TileData> tileMaster = new List<TileData>();

    public bool basicShape = true;
    
    public Count wallCount = new Count(5, 9); // minimum and maximum of walls
    public Count foodCount = new Count(1, 3);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] outFloorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    
    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3> ();

    public GameObject[] pisces;

    void InitialiseList()
    {

        // Clear list
        gridPositions.Clear ();

            int counter = 0;
            // create legitimate play area
        
            // Loop through cols
            for (int x = 0; x < columns + 1; x++)
            {
                // Loop through rows
                for (int y = 0; y < rows + 1; y++)
                {
                    // At each index add a new Vector3 
                    
                    gridPositions.Add(new Vector3(x, y, 0f));
                    
                    counter += 1;
                }
            }
    }

    void BoardSetup ()
    {

        tileMaster.Clear();
        boardHolder = new GameObject("Board").transform;
        int counter = 0;
        // Generate entire field
    
            for (int x = 0; x < columns+1; x++)
            {
                for (int y = 0; y < rows+1; y++)
                {
                    tileMaster.Add(new TileData(counter, x, y));
             
                    GameObject toInstantiate = null;
         
                    // outer limits get outermost tiles
                    //if (x == -1 || x == columns || y == -1 || y == rows)
                    //{
                    //   toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    //}

                        // leave corners out to make more treetrunklike level
                        if (( x < 2 && y == 0) || (x > columns - 2 && y == 0) || (x < 2 && y == rows) || (x > columns - 2 && y == rows))
                        {

                        }
                        else
                        {
                     
                  
                      
                 

                        // See if new tile is within pisces limits

                        foreach (GameObject piscesSphere in pisces)
                        {           
                            RaycastHit hit;
                            if (Physics.Raycast(new Vector3(tileMaster[counter].x, tileMaster[counter].y, 0), -Vector3.forward, out hit))
                            {
                                // What to do if in bounds
                                //Component[] s = instance.transform.GetComponents<MyTileObject>();
                                //foreach (Component com in s)
                                //{
                                    //com.GetComponent<MyTileObject>().inMonastery = true;
                                    tileMaster[counter].setIsMonastery(true);
                                    if (tileMaster[counter].hasTile == false)
                                    {
                                        toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                                        GameObject instance = Instantiate(toInstantiate, new Vector3(tileMaster[counter].x, tileMaster[counter].y, 0f), Quaternion.identity) as GameObject;
                                        instance.transform.SetParent(boardHolder);

                                        int colorChoice = Random.Range(0, 5);
                                        if(colorChoice == 0)
                                        {

                                        } else if (colorChoice == 1)
                                        {
                                            // green
                                            instance.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.2f, 1.0f);
                                        } else if (colorChoice == 2)
                                        {
                                            // cyan
                                            instance.GetComponent<SpriteRenderer>().color = new Color(0.05f, 0.95f, 0.9f, 1.0f);
                                        } else if (colorChoice == 3)
                                        {
                                            // ochre
                                            instance.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.1f, 1.0f);
                                        } else if (colorChoice == 4)
                                        {
                                            // yellow
                                            instance.GetComponent<SpriteRenderer>().color = new Color(0.95f, 0.92f, 0.05f, 1.0f);
                                        }

                                        
                                        tileMaster[counter].hasTile = true;
                                    }

                            }
                            else
                            {
                                if (tileMaster[counter].hasTile == false)
                                {
                                    toInstantiate = outFloorTiles[Random.Range(0, outFloorTiles.Length)];
                                    GameObject instance = Instantiate(toInstantiate, new Vector3(tileMaster[counter].x, tileMaster[counter].y, 0f), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(boardHolder);
                                    tileMaster[counter].hasTile = true; 
                                }
                        }
                        }
                    }
                    counter += 1;
                }
            }

        

        // Now see if we can find the outer walls of the monastery within the pisces 
        //
         
        for (int c = 0; c < tileMaster.Count; c++)
        {
            int[] neighbours = new int[8];
            TileData aboveTile, belowTile, leftTile, rightTile = null;
            
                //Debug.Log("this is " + tileMaster[c].x + " " + tileMaster[c].y + " reporting in");
                
                // find tiles above, below, left and right
                // rules:
                // if all four sides are in monastery, then definitely not an outer wall

                // GROUP 
                // using lambda for the first time ever...
                // Store neighbour awareness into each tile
                List<TileData> sameColumn = tileMaster.FindAll(TileData => TileData.x == tileMaster[c].x);
                List<TileData> priorColumn = tileMaster.FindAll(TileData => TileData.x == tileMaster[c].x-1);
                List<TileData> nextColumn = tileMaster.FindAll(TileData => TileData.x == tileMaster[c].x+1);
                List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == tileMaster[c].y);

                // up left tile
                try
                {
                   tileMaster[c].nbTiles[0] = priorColumn.Find(TileData => TileData.y == (tileMaster[c].y + 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[0] = -1; }

                // above tile
                try
                {
                    tileMaster[c].nbTiles[1] =  sameColumn.Find(TileData => TileData.y == (tileMaster[c].y + 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[1] = -1; }

                // up right tile
                try
                {
                    tileMaster[c].nbTiles[2] = nextColumn.Find(TileData => TileData.y == (tileMaster[c].y + 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[2] = -1; }

                // left tile
                try
                {
                    tileMaster[c].nbTiles[3] = sameRow.Find(TileData => TileData.x == (tileMaster[c].x - 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[3] = -1; }

                // right tile
                try
                {
                    tileMaster[c].nbTiles[4] = sameRow.Find(TileData => TileData.x == (tileMaster[c].x + 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[4] = -1; }

                // up left tile
                try
                {
                    tileMaster[c].nbTiles[5] = priorColumn.Find(TileData => TileData.y == (tileMaster[c].y - 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[5] = -1; }

                try
                {
                    tileMaster[c].nbTiles[6] = sameColumn.Find(TileData => TileData.y == (tileMaster[c].y - 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[6] = -1; }

                try
                {
                    tileMaster[c].nbTiles[7] = nextColumn.Find(TileData => TileData.y == (tileMaster[c].y - 1)).getIndex();
                }
                catch { tileMaster[c].nbTiles[7] = -1; }


           
            if (tileMaster[c].isMonastery()) // && !tileMaster[c].isChecked())
            {
                // All 
                if ((tileMaster[tileMaster[c].nbTiles[1]].isMonastery() && tileMaster[tileMaster[c].nbTiles[4]].isMonastery() && tileMaster[tileMaster[c].nbTiles[3]].isMonastery() && tileMaster[tileMaster[c].nbTiles[6]].isMonastery()))
                {
                    // Definitely not a monastery outer wall if all sides are surrounded by monastery      
                }
                else
                {
                    GameObject instance =  Instantiate(testWall, new Vector3(tileMaster[c].x, tileMaster[c].y, 0), Quaternion.identity) as GameObject;
    
                    instance.transform.SetParent(boardHolder);
                    tileMaster[c].setMonasteryWall(true);
                    
                    tileMaster[c].monasteryWallObject = instance;
                    tileMaster[c].boxCollider = instance.GetComponent<BoxCollider2D>();
                    //checkDirection(1, 0, 15, c);
                }
            }
            tileMaster[c].setChecked(true);
        }

        // What about the interior walls?
        // Should be able to use raycasting here to bounce off interior walls

        // Some kind of rules such that rays from tiles past the half-point can only go left,
        // rays below the midpoint only go up etc
        //tileMaster[295].showNeighbours(tileMaster);
        // 
        for (int g = 0; g < 8; g++ )
        {
            Instantiate(testWall, new Vector3(tileMaster[tileMaster[295].nbTiles[g]].x, tileMaster[tileMaster[295].nbTiles[g]].y, 0), Quaternion.identity);
            tileMaster[tileMaster[295].nbTiles[g]].setMonasteryWall(true);
        }


            if (innerWalls)
            {
                for (int c = 1; c < (columns * rows); c++)
                {
                    if (tileMaster[c].isMonasteryWall())
                    {
                        if (Random.Range(0, 10) < 1)
                        {
                            int dir = 1;
                            if (tileMaster[c].x > columns / 2)
                                dir = -1;

                            Debug.Log("Casting ray");

                            int xDir = 1;
                            int yDir = 0;
                            if (Random.Range(0, 2) < 1)
                            {
                                xDir = 0;
                                yDir = 1;
                            }
                            checkDirection(xDir, yDir, dir * 15, c);

                            for (int d = 1; d < 10; d++)
                            {
                                GameObject instance = Instantiate(testWall, new Vector3(tileMaster[c].x + d * dir, tileMaster[c].y, 0), Quaternion.identity) as GameObject;

                            }

                        }
                    }
                }

            }



        yoghurtBehaviour();
    }

    void yoghurtBehaviour()
    {

        // reset growth allowance
        for (int i = 0; i < tileMaster.Count; i++)
        {
            tileMaster[i].grownThisTurn = false;    
        }

        tileMaster[444].yoghurtOnTile = Instantiate(yoghurt, new Vector3(tileMaster[444].x, tileMaster[444].y, 0), Quaternion.identity) as GameObject;
        
        for (int i = 0; i < tileMaster.Count; i++)
        {
            if(tileMaster[i].yoghurtOnTile != null)
            {
                tileMaster[i].yoghurtGrow(tileMaster, yoghurt);
            }
        }


    }


    bool checkDirection(int xDir, int yDir, int range, int tileIndex)
    {
        
        Vector2 start = new Vector2(tileMaster[tileIndex].monasteryWallObject.transform.position.x, tileMaster[tileIndex].monasteryWallObject.transform.position.y);
        Vector2 end = start + new Vector2(xDir, yDir) * range;

        // Make sure we don't hit our own collider when casting ray
        tileMaster[tileIndex].boxCollider.enabled = false;
        Debug.DrawRay(start, new Vector2(xDir, yDir) * range, Color.green, 20, false);
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        tileMaster[tileIndex].boxCollider.enabled = true;

        if (hit.transform == null)
        {
            //
            //
            return true;
        }
        return false;
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(1, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;

    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }
    }

    public void SetupScene(int level)
    {
        pisces = GameObject.FindGameObjectsWithTag("Pisces");

        BoardSetup();
        InitialiseList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f); // logarithmic. 3 enemies, level 8, etc.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }

}
