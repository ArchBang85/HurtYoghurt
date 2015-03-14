using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // Need to specify because there's an overlap between System and UnityEngine

//namespace Completed;

public class BoardManager : MonoBehaviour {

    public GameObject testWall;
    //public GameObject yoghurt;
    public GameObject yoghurtHolder;
    public GameObject[] yoghurtTypes = new GameObject[5];
    public LayerMask blockingLayer;
    public GameObject playerObject;
    public bool innerWalls = false;
    private bool yoghurtStart = true;
    public GameObject[] particleEffects = new GameObject[3];

    void Awake()
    {
        playerObject = GameObject.Find("Player");
        yoghurtHolder = GameObject.Find("YoghurtHolder");
    }

    void Start()
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

        if (Input.GetKeyDown(KeyCode.Y))
        {
          //  Debug.Log(RandomTileInMonastery());
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            yoghurtBehaviour();
        }

        // Acididc
        if(Input.GetKeyDown(KeyCode.O))
        {
            areaEffect(0, 1, 600, tileMaster, -1);
        }


        if(Input.GetKeyDown(KeyCode.P))
        {
            areaEffect(0, 1, 600, tileMaster, 1);
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
        public GameObject myFloor = null;
        public GameObject myYoghurt;
        public bool isExit = false;

        public string description;
        public int floorType = 0;
        private int floorTypeMax = 4;

        public int yoghurtLevel = 0;
        private int yoghurtLevelMax = 5;

        public bool grownThisTurn = false;
        public int growthTimer = 0;
        public int growthTimerReset = 1;     

        public void updateColour()
        {
            if (this.isMonasteryTile)
            {
                // Allow yoghurtlevel to loop around 
                if (floorType > floorTypeMax)
                {
                    floorType = floorTypeMax;
                }
                else if (floorType < 0)
                {
                    floorType = 0;
                }

                if (floorType == 0)
                {
                    // green
                    myFloor.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.2f, 1.0f);
                }
                else if (floorType == 1)
                {
                    // cyan
                    myFloor.GetComponent<SpriteRenderer>().color = new Color(0.05f, 0.95f, 0.9f, 1.0f);

                }
                else if (floorType == 2)
                {
                    // standard basic floor
                    myFloor.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
                }
                else if (floorType == 3)
                {
                    // yellow
                    myFloor.GetComponent<SpriteRenderer>().color = new Color(0.95f, 0.92f, 0.05f, 1.0f);
                }
                else if (floorType == 4)
                {

                    // ochre
                    myFloor.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.1f, 1.0f);
                }
            }
        }
       
        
    
       
        void spinObject(GameObject obj)
        {

            int rotateChance = Random.Range(0, 10);
            if (rotateChance < 5)
            {
                int rotationAmount = 90;
                if (Random.Range(0, 2) < 1)
                {
                    rotationAmount = 45;
                }
                obj.transform.Rotate(-Vector3.forward, rotationAmount, Space.World);
            }

        }

        public void yoghurtIncrement()
        {
            if(growthTimer < 0)
            {
                yoghurtLevel += 1;
                growthTimer = growthTimerReset;
            }
            growthTimer -= 1;


        }
        public void updateYoghurt(GameObject[] yogTypes, GameObject yoghurtHolder)
        {
            if (yoghurtLevel > 5) yoghurtLevel = 5;
            if (yoghurtLevel < 0) yoghurtLevel = 0;

  
                // What happens should depend on floor type
            
                if(yoghurtLevel == 0)
                {
                    Destroy(myYoghurt);
                    myYoghurt = null;
                }

                if(yoghurtLevel == 1)
                {
                    // basic yoghurt level, one splodge
                    if(myYoghurt == null)
                    {
             
                        // find out about rotation opportunities 
                        GameObject yogInstance = Instantiate(yogTypes[0], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
              
                        myYoghurt = yogInstance;
                        myYoghurt.layer = LayerMask.NameToLayer("Default");
  
                        if (Random.Range(0, 10) < 5)
                        {
                            spinObject(yogInstance);
                        }
                        //yogInstance.transform.SetParent(yoghurtHolder.transform);

                    }
                } else if (yoghurtLevel == 2)
                {
                    // remove previous splodge
                        Destroy(myYoghurt);
                        myYoghurt = null;
                        // two splodges
                       // Debug.Log("creating double splodge");
                        GameObject yogInstance = Instantiate(yogTypes[1], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        
                
                        myYoghurt = yogInstance;
                        myYoghurt.layer = LayerMask.NameToLayer("Default");

                        if (Random.Range(0, 10) < 5)
                        {
                            spinObject(yogInstance);
                        }
                        //yogInstance.transform.SetParent(yoghurtHolder.transform);

                    //}
                } else if (yoghurtLevel == 3)
                {
                    // three splodges
                    // remove previous splodge
                    Destroy(myYoghurt);
                    myYoghurt = null;
                    // two splodges
                  //  Debug.Log("creating triple splodge");
                    GameObject yogInstance = Instantiate(yogTypes[2], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    myYoghurt = yogInstance;
                    myYoghurt.layer = LayerMask.NameToLayer("Default"); 
                    if (Random.Range(0, 10) < 5)
                    {
                        spinObject(yogInstance);
                    }
                    //yogInstance.transform.SetParent(yoghurtHolder.transform);

                } else if (yoghurtLevel == 4)
                {
                    // Getting yoghurty!
                    // Movement will slow
                    Destroy(myYoghurt);
                    myYoghurt = null;
                    // two splodges
                  //  Debug.Log("creating triple splodge");
                    GameObject yogInstance = Instantiate(yogTypes[3], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    myYoghurt = yogInstance;
                    myYoghurt.layer = LayerMask.NameToLayer("Default");
                    if (Random.Range(0, 10) < 5)
                    {
                        spinObject(yogInstance);
                    }
                } else if (yoghurtLevel == 5)
                {
                    // MAXIMUM YOGHURT
                    // full sprite, can no longer travel
                    Destroy(myYoghurt);
                    myYoghurt = null;
                    // two splodges
                  //  Debug.Log("creating maximum splodge");
                    // Make impassable
               


                    GameObject yogInstance = Instantiate(yogTypes[4], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    myYoghurt = yogInstance;
                    myYoghurt.layer = LayerMask.NameToLayer("BlockingLayer");
                    if (Random.Range(0, 10) < 5)
                    {
                        spinObject(yogInstance);
                    }
                }
            
           
 

        }

        public bool hasItems = false;
        // neighbouring tiles as integers
        // 0 1 2 
        // 3 c 4
        // 5 6 7

        public int[] nbTiles = new int[8];

        // constructor
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
    public GameObject relic;
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

    /// <summary>
    /// MAP GENERATION
    /// </summary>
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
                                // Create monastery floor tile
                                tileMaster[counter].setIsMonastery(true);
                                if (tileMaster[counter].hasTile == false)
                                {
                                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                                    GameObject instance = Instantiate(toInstantiate, new Vector3(tileMaster[counter].x, tileMaster[counter].y, 0f), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(boardHolder);                                        
                                    tileMaster[counter].hasTile = true;
                                    tileMaster[counter].myFloor = instance;   
                                    
                                    // Create basic floor type
                                    tileMaster[counter].floorType = 2;
                                    
                                    tileMaster[counter].updateColour();

                                }

                            }
                            else
                            {
                                if (tileMaster[counter].hasTile == false)
                                {
                                    // Create tile outside monastery boundaries
                                    toInstantiate = outFloorTiles[Random.Range(0, outFloorTiles.Length)];
                                    GameObject instance = Instantiate(toInstantiate, new Vector3(tileMaster[counter].x, tileMaster[counter].y, 0f), Quaternion.identity) as GameObject;
                                    instance.transform.SetParent(boardHolder);
                                    tileMaster[counter].hasTile = true;
                                    tileMaster[counter].myFloor = instance;
                                    // make the out-of-monastery floor tiles a little transparent for effect
                                    instance.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

                                }
                            }
                        }
                    }
                    counter += 1;
                }
            }

        // KEEP TRACK OF EXTREMITIES OF MONASTERY
            int monasteryLeftmostX = 1000;
            int monasteryRightmostX = 0;
            int monasteryTopmostY = 0;
            int monasteryBottommostY = 1000;

        // HACK
            int bottomDoorX = 0;
            int topDoorX = 0;

        // Now see if we can find the outer walls of the monastery within the pisces          
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


           
            if (tileMaster[c].isMonastery()) 
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

                    // Check if leftmost or rightmost
                    if (tileMaster[c].x < monasteryLeftmostX)
                    {
                        monasteryLeftmostX = tileMaster[c].x;
                    }
                    if (tileMaster[c].x > monasteryRightmostX)
                    {
                        monasteryRightmostX = tileMaster[c].x;
                    }
                    if (tileMaster[c].y > monasteryTopmostY)
                    {
                        monasteryTopmostY= tileMaster[c].y;
                    }
                    if (tileMaster[c].y < monasteryBottommostY)
                    {
                        monasteryBottommostY = tileMaster[c].y;
                    }

                    //checkDirection(1, 0, 15, c);
                }
            }
            tileMaster[c].setChecked(true);
        }

        // INTERIOR WALLS

        Debug.Log(monasteryLeftmostX);
        Debug.Log(monasteryRightmostX);

        // ROUGH HACK
        int mainWallTopY = 21;
        int mainWallBottomY = 8;


        // vertical partitions
        int partitions = Random.Range(4, 8);
        int coolDown = 2;
        for (int xPos = monasteryLeftmostX + 1; xPos < monasteryRightmostX; xPos++ )
        {
            
            if(partitions > 0)
            {
                coolDown -= 1;

                if (coolDown < 0)
                {
                    // chance to split
                    if (Random.Range(0, 10) < 2)
                    {
                        int doorWays = Random.Range(1, 3);
                        int minimalDoorway = Random.Range(mainWallBottomY + 1, mainWallTopY);
                        // Create vertical partition
                        for (int yPos = mainWallBottomY + 1; yPos < mainWallTopY; yPos++)
                        {
                            // Has to have doorways too
                            if (yPos == minimalDoorway)
                            {
                                // ATM do nothing if there's a doorway
                            }
                            else
                            {

                                GameObject instance = Instantiate(testWall, new Vector3(xPos, yPos, 0), Quaternion.identity) as GameObject;

                                instance.transform.SetParent(boardHolder);
                                // Find tilemaster index

                                List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == yPos);
                                // Players tile on tilemap
                                int activeTileIndex = sameRow.Find(TileData => TileData.x == xPos).getIndex();
                                tileMaster[activeTileIndex].setMonasteryWall(true);
                                tileMaster[activeTileIndex].monasteryWallObject = instance;
                                tileMaster[activeTileIndex].boxCollider = instance.GetComponent<BoxCollider2D>();
                            }
                        }

                            partitions -= 1;
                            coolDown = 2;

                    }
                }
             

            }
        }

        // horisontal partitions
        // HACK

        // 

        Debug.Log(monasteryBottommostY);
        Debug.Log(monasteryTopmostY);

        // Find sample tiles in extreme top and bottom to know where to place doorways
        for (int c = 0; c < tileMaster.Count; c++)
        {
            if(tileMaster[c].isMonasteryWall() && tileMaster[c].y == monasteryBottommostY)
            {
                bottomDoorX = tileMaster[c].x;
                c = tileMaster.Count;
            }
        }
        for (int c = 0; c < tileMaster.Count; c++)
        {
            if (tileMaster[c].isMonasteryWall() && tileMaster[c].y == monasteryTopmostY)
            {

                topDoorX = tileMaster[c].x;
                c = tileMaster.Count;
            }
        }


        for (int xPos = monasteryLeftmostX + 4; xPos < monasteryRightmostX - 5; xPos++)
        {
            if (xPos != topDoorX)
            {
                GameObject instance = Instantiate(testWall, new Vector3(xPos, mainWallTopY, 0), Quaternion.identity) as GameObject;
               // GameObject instance2 = Instantiate(testWall, new Vector3(xPos + 1, mainWallTopY, 0), Quaternion.identity) as GameObject;

                
            }
            else
            {
                xPos += 1;
            }
        }
        for (int xPos = monasteryLeftmostX + 4; xPos < monasteryRightmostX - 5; xPos++)
        {
            if (xPos != bottomDoorX)
            {
                GameObject instance = Instantiate(testWall, new Vector3(xPos, mainWallBottomY, 0), Quaternion.identity) as GameObject;
                // GameObject instance2 = Instantiate(testWall, new Vector3(xPos + 1, mainWallTopY, 0), Quaternion.identity) as GameObject;


            }
            else
            {
                xPos += 1;
            }
        }

        //GameObject instance4 = Instantiate(testWall, new Vector3(xPos + 1, mainWallBottomY, 0), Quaternion.identity) as GameObject;

            // What about the interior walls?
            // Should be able to use raycasting here to bounce off interior walls

            // Some kind of rules such that rays from tiles past the half-point can only go left,
            // rays below the midpoint only go up etc
            //tileMaster[295].showNeighbours(tileMaster);
            // 
            /*for (int g = 0; g < 8; g++ )
            {
                Instantiate(testWall, new Vector3(tileMaster[tileMaster[295].nbTiles[g]].x, tileMaster[tileMaster[295].nbTiles[g]].y, 0), Quaternion.identity);
                tileMaster[tileMaster[295].nbTiles[g]].setMonasteryWall(true);
            }*/


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



       // yoghurtBehaviour();
    }

    // Growing an individual yoghurt
    public void yoghurtGrow(int tileIndex)
        {
            // Remember to also learn how to remove this
            if(tileMaster[tileIndex].myYoghurt != null && !tileMaster[tileIndex].grownThisTurn)
            {  


               // Growth depends on tile type

                // Type 0 growth: Very Acidic

                // Type 1 growth: Acidic

                if (tileMaster[tileIndex].floorType == 1)
                {
                    // If the tile is full of yoghurt it will blow

                    if(tileMaster[tileIndex].yoghurtLevel == 5)
                    {
                        tileMaster[tileIndex].yoghurtLevel = 0;
                        tileMaster[tileIndex].updateYoghurt(yoghurtTypes, yoghurtHolder);
                        tileMaster[tileIndex].floorType += 1;
                        tileMaster[tileIndex].updateColour();
                        // Wipeout nearby yoghurt
                        for (int n = 0; n < 8; n++)
                        {
                            tileMaster[tileMaster[tileIndex].nbTiles[n]].yoghurtLevel = 0;
                            tileMaster[tileMaster[tileIndex].nbTiles[n]].updateYoghurt(yoghurtTypes, yoghurtHolder);

                        }

                        // Have a chance of spreading yoghurt in tiles beyond the near ones
                        for (int n = 0; n < 8; n++)
                        {
                            int tTile = tileMaster[tileIndex].nbTiles[n];
                            int targetTileTwoRemoved = tileMaster[tTile].nbTiles[n];
                            if(tileMaster[targetTileTwoRemoved].isMonastery() && !tileMaster[targetTileTwoRemoved].isMonasteryWall())
                            {
                                if (Random.Range(0, 10) < 2)
                                {
                                    tileMaster[targetTileTwoRemoved].yoghurtIncrement();
                                    tileMaster[targetTileTwoRemoved].updateYoghurt(yoghurtTypes, yoghurtHolder);
                                }
                            }

                        }
                            // Create explosion effect
                            Instantiate(particleEffects[0], new Vector3(tileMaster[tileIndex].x, tileMaster[tileIndex].y, 0), Quaternion.identity);
                            Instantiate(particleEffects[1], new Vector3(tileMaster[tileIndex].x, tileMaster[tileIndex].y, 0), Quaternion.identity);
                    }

                }
                // Type 2 growth: Basic = random one per tile

                if (tileMaster[tileIndex].floorType == 2)
                {
                
                    // Get randomised cardinal directions
                    int r = Random.Range(1, 5);
                    if (r == 2)
                        r = 6;

                    int targetTile = tileMaster[tileIndex].nbTiles[r];

                    // Catch cases where tile doesn't exist
                    if (targetTile != -1)
                    {
                        // isn't growing on a wall
                        if (!tileMaster[targetTile].isMonasteryWall())
                        {
                            // Make sure yoghurt doesn't exist on tile already
                            if (tileMaster[targetTile].myYoghurt == null)
                            {
                                // Make sure can't grow more than once a turn
                                if (!tileMaster[targetTile].grownThisTurn)
                                {
                                    // Different growth processes

                                    //tileMaster[targetTile].myYoghurt = Instantiate(yoghurtTypes[], new Vector3(tileMaster[targetTile].x, tileMaster[targetTile].y, 0), Quaternion.identity) as GameObject;
                                    tileMaster[targetTile].yoghurtIncrement();
                                    tileMaster[targetTile].grownThisTurn = true;
                                    tileMaster[targetTile].updateYoghurt(yoghurtTypes, yoghurtHolder);
                                }
                            }
                            else
                            // pre-existing yoghurt
                            {
                               
                                // yoghurt is beyond level 1
                                if (!tileMaster[targetTile].grownThisTurn)
                                {
                                    // Different growth processes

                                    //tileMaster[targetTile].myYoghurt = Instantiate(yoghurtTypes[], new Vector3(tileMaster[targetTile].x, tileMaster[targetTile].y, 0), Quaternion.identity) as GameObject;
                                    tileMaster[targetTile].yoghurtIncrement();
                                    tileMaster[targetTile].grownThisTurn = true;
                                    tileMaster[targetTile].updateYoghurt(yoghurtTypes, yoghurtHolder);
                                }
                            }
                        }
                    }
                }

                // Type 3 growth: Alkaline

                // Type 4 growth: Very Alkaline

            }
        }

    void yoghurtBehaviour()
    {

        // reset growth allowance
        for (int i = 0; i < tileMaster.Count; i++)
        {
            tileMaster[i].grownThisTurn = false;    
        }

        // set initial yoghurt, do only once
        if (yoghurtStart)
        {
            // Should be randomised and depend on level
            tileMaster[444].myYoghurt = Instantiate(yoghurtTypes[0], new Vector3(tileMaster[444].x, tileMaster[444].y, 0), Quaternion.identity) as GameObject;
            yoghurtStart = false;
        }

        // Go through each tile once and calculate yoghurt behaviour
        for (int i = 0; i < tileMaster.Count; i++)
        {
                yoghurtGrow(i);
        }
    }

    void areaEffect(int areaEffectType, int range, int tileIndex, List<TileData>tM,  int areaEffectImpact = -1)
    {
        // Get player coordinates
        int x = (int)playerObject.transform.position.x;
        int y = (int)playerObject.transform.position.y;

        List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == y);
        // Players tile on tilemap
        int activeTileIndex = sameRow.Find(TileData => TileData.x == x).getIndex();

        // Types of effects:
        // Either impact FLOOR or YOGHURT

        // 0 FLOOR
        // 1 YOGHURT

        // // // // // // // 
        // FLOOR IMPACT   //
        // // // // // // //

        if (areaEffectType == 0)
        {
            if (range == 1)
            {
                // Do own tile
                tM[activeTileIndex].floorType += areaEffectImpact;
                tM[activeTileIndex].updateColour();

                // Do immediately surrounding tiles
                for (int g = 0; g < 8; g++)
                {
                   if(tM[tM[activeTileIndex].nbTiles[g]].isMonastery())
                   { 
                    
                        // Floor can either increment floortype up or down
                        tM[tM[activeTileIndex].nbTiles[g]].floorType += areaEffectImpact;
                        tM[tM[activeTileIndex].nbTiles[g]].updateColour();
                  }
                }

                // Do the tiles two above, two left, two right, two below
                int upperTile = tM[activeTileIndex].nbTiles[1];
                // But block if monastery wall
                if(!tM[upperTile].isMonasteryWall())
                { 
                    tM[tM[upperTile].nbTiles[1]].floorType += areaEffectImpact;
                    tM[tM[upperTile].nbTiles[1]].updateColour();
                }
                int leftTile = tM[activeTileIndex].nbTiles[3];
                if (!tM[leftTile].isMonasteryWall())
                {
                  
                    tM[tM[leftTile].nbTiles[3]].floorType += areaEffectImpact;
                    tM[tM[leftTile].nbTiles[3]].updateColour();
                }
                int rightTile = tM[activeTileIndex].nbTiles[4];
                if (!tM[rightTile].isMonasteryWall())
                {
                    tM[tM[rightTile].nbTiles[4]].floorType += areaEffectImpact;
                    tM[tM[rightTile].nbTiles[4]].updateColour();
                }
                int lowerTile = tM[activeTileIndex].nbTiles[6];
                if (!tM[lowerTile].isMonasteryWall())
                {  
                   
                    tM[tM[lowerTile].nbTiles[6]].floorType += areaEffectImpact;
                    tM[tM[lowerTile].nbTiles[6]].updateColour();
                }
            }

            if (areaEffectImpact == 1)
            {
                if(Random.Range(0,10)<5)
                {
                    this.GetComponent<LogManager>().logMessage("The lye spreads quickly.");

                }
                else
                {
                    this.GetComponent<LogManager>().logMessage("You splash the lye around you.");
                }
                
            } else if (areaEffectImpact == -1)
            {
                if (Random.Range(0, 10) < 5)
                {
                    this.GetComponent<LogManager>().logMessage("You pour treasured wine on the ground.");

                }
                else
                {
                    this.GetComponent<LogManager>().logMessage("A steady stream of wine swiftly covers a large area.");
                }
            }

        } else if (areaEffectType == 1)
        {

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

    public bool checkSurrounded(int tileIndex)
    {
        // if fully surrounded, then dies
        int counter = 0;
        for (int t = 0; t < 8; t++)
        {
            if(tileMaster[tileMaster[tileIndex].nbTiles[t]].yoghurtLevel == 5)
            {
                counter += 1;
            }
        }

        if (counter == 8)
        {
            return true;
        }
        return false;
    }

    public bool checkDiagonal(int x, int y, Vector3 playerPos)
    {
        int targetX = (int)playerPos.x + x;
        int targetY = (int)playerPos.y + y;

        // Allow when not a wall / monastery door and when within the bounds
        List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == targetY);
        // Players tile on tilemap
        int activeTileIndex = sameRow.Find(TileData => TileData.x == targetX).getIndex();

        // Legit moves for the moment are within the monastery and not walls
        if(tileMaster[activeTileIndex].isMonastery() && !tileMaster[activeTileIndex].isMonasteryWall())
        {
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

    int RandomTileInMonastery()
    {
        bool spaceFound = false;
        while (!spaceFound)
        {
            int randomIndex = Random.Range(1, gridPositions.Count);
            List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == gridPositions[randomIndex].y);
            // Players tile on tilemap
            int activeTileIndex = sameRow.Find(TileData => TileData.x == gridPositions[randomIndex].x).getIndex();
            if(tileMaster[activeTileIndex].isMonastery() && !tileMaster[activeTileIndex].isMonasteryWall())
            {
                spaceFound = true;
                return activeTileIndex;
            }
        }
        return -1;

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

    void placeExit()
    {
        int exitTile = RandomTileInMonastery();
        Instantiate(exit, new Vector3(tileMaster[exitTile].x, tileMaster[exitTile].y, 0f), Quaternion.identity);
    }

    void placeRelics(int relicCount)
    {
        for (int r = 0; r < relicCount; r++)
        {
            int targetTile = RandomTileInMonastery();
            Instantiate(relic, new Vector3(tileMaster[targetTile].x, tileMaster[targetTile].y, 0f), Quaternion.identity);
        }

    }

    public void SetupScene(int level)
    {
        pisces = GameObject.FindGameObjectsWithTag("Pisces");

        BoardSetup();
        InitialiseList();
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        


        int relicCount = (int)Mathf.Log(level, 2f); // logarithmic. 3 on level 8, etc.
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        relicCount = 5;

        // Place relics
        placeRelics(relicCount);

        // Put exit within bounds of monastery
        placeExit();
    }
}
