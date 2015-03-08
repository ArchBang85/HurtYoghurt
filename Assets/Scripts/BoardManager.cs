using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // Need to specify because there's an overlap between System and UnityEngine

//namespace Completed;

public class BoardManager : MonoBehaviour {
        
    public GameObject testWall;
    public LayerMask blockingLayer;
    public bool innerWalls = false;
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

    public class TileData
    {
        
        int index {get; set;}
        public int x { get; set; }
        public int y { get; set; }
        bool beenChecked = false;
        bool monasteryWall = false;
        public BoxCollider2D boxCollider;
        public GameObject monasteryWallObject = null;
        public string description;
        public int yoghurtLevel = 0;
        public bool hasItems = false;

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



    }

    public int columns = 8; 
    public int rows = 8;
    public List<TileData> tileMaster = new List<TileData>();

    public bool basicShape = true;
    
    public Count wallCount = new Count(5, 9); // minimum and maximum of walls
    public Count foodCount = new Count(1, 3);
    public GameObject exit;
    public GameObject[] floorTiles;
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
                 
                 
                        GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    
                   

                    
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
                            GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                            instance.transform.SetParent(boardHolder);
                      
                 

                        // See if new tile is within pisces limits

                        foreach (GameObject piscesSphere in pisces)
                        {           
                            RaycastHit hit;
                            if (Physics.Raycast(instance.transform.position, -Vector3.forward, out hit))
                            {
                                // What to do if in bounds
                                //Component[] s = instance.transform.GetComponents<MyTileObject>();
                                //foreach (Component com in s)
                                //{
                                    //com.GetComponent<MyTileObject>().inMonastery = true;
                                    tileMaster[counter].setIsMonastery(true);
                                //}
                            }
                        }
                    }
                    counter += 1;
                }
            }

        

        // Now see if we can find the outer walls of the monastery within the pisces 
        //
        for (int c = 1; c < (columns * rows); c++)
        {
            TileData aboveTile, belowTile, leftTile, rightTile = null;
            if(tileMaster[c].isMonastery()) // && !tileMaster[c].isChecked())
            {
                //Debug.Log("this is " + tileMaster[c].x + " " + tileMaster[c].y + " reporting in");
                
                // find tiles above, below, left and right
                // rules:
                // if all four sides are in monastery, then definitely not an outer wall

                // GROUP 
                // using lambda for the first time ever...
                
                List<TileData> sameColumn = tileMaster.FindAll(TileData => TileData.x == tileMaster[c].x);

                try
                {
                    aboveTile = sameColumn.Find(TileData => TileData.y == (tileMaster[c].y + 1));
                }
                catch { aboveTile = null; }
                try
                {
                    belowTile = sameColumn.Find(TileData => TileData.y == (tileMaster[c].y - 1));
                }
                catch { belowTile = null; }
   
                List<TileData> sameRow = tileMaster.FindAll(TileData => TileData.y == tileMaster[c].y);
                 try
                {
                    leftTile = sameRow.Find(TileData => TileData.x == (tileMaster[c].x - 1));
                }
                 catch { leftTile = null; }

                try
                {
                    rightTile = sameRow.Find(TileData => TileData.x == (tileMaster[c].x + 1));
                }
                catch { rightTile = null; }

                /*
                try
                {
                    Debug.Log("above me x: " + aboveTile.x);
                    Debug.Log("above me y: " + aboveTile.y);
                }
                catch { }
                try
                {
                    Debug.Log("below me x: " + belowTile.x);
                    Debug.Log("below me y: " + belowTile.y);
                }
                catch { }
                try
                {
                    Debug.Log("left of me x: " + leftTile.x);
                    Debug.Log("left of me y: " + leftTile.y);
                }
                catch{}
                try{
                    Debug.Log("right of me x: " + rightTile.x);
                    Debug.Log("right of me y: " + rightTile.y);
                }
                catch { }
                */

                // All 
                if ((aboveTile.isMonastery() && rightTile.isMonastery() && leftTile.isMonastery() && belowTile.isMonastery()))
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
        
        // 
        if(innerWalls)
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
                    if (Random.Range(0,2)<1)
                    {
                        xDir = 0;
                        yDir = 1;
                    }
                    checkDirection(xDir, yDir, dir * 15, c);
                    
                    for(int d = 1; d < 10; d++)
                    {
                        GameObject instance = Instantiate(testWall, new Vector3(tileMaster[c].x + d*dir, tileMaster[c].y, 0), Quaternion.identity) as GameObject;
                        
                    }

                }
            }
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
