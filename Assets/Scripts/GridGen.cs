using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public enum Directions
{
    FRONT,
    BACK,
    LEFT,
    RIGHT
}


public class Tile
{
    GameObject self;
    
    GameObject leftWall, rightWall, backWall, frontWall, pillarBR, pillarFR, pillarBL, pillarFL;

    public Tile frontNeighbor { get; set; }
    public Tile backNeighbor { get; set; }
    public Tile rightNeighbor { get; set; }
    public Tile leftNeighbor { get; set; }

    public bool visited { get; set; }

    public bool rightVisit { get; set; } = false;
    public bool leftVisit { get; set; } = false;

    public string coord { get; set; } = "";

    public GameObject floor { get; set; }

    // reference to previous Tile (used for Wilson's algorithim) - eliminates need for stack
    public Tuple<Tile,Directions?> nextCell { get; set; }

    public Tile(GameObject self)
    {
        // need reference to game object this tile corresponds to
        this.self = self;

        // get references to each piece of prefab
        leftWall = self.transform.Find("WallTileLeft").gameObject;
        rightWall = self.transform.Find("WallTileRight").gameObject;
        frontWall = self.transform.Find("WallTileFront").gameObject;
        backWall = self.transform.Find("WallTileBack").gameObject;
        pillarBR = self.transform.Find("BackRightPillar").gameObject;
        pillarFR = self.transform.Find("FrontRightPillar").gameObject;
        pillarBL = self.transform.Find("BackLeftPillar").gameObject;
        pillarFL = self.transform.Find("FrontLeftPillar").gameObject;
        floor = self.transform.Find("TileFloor").gameObject;

        

        // prefab will always have at least two neighbors
        // isn't known at compile time, however
        frontNeighbor = null;
        backNeighbor = null;
        rightNeighbor = null;
        leftNeighbor = null;

        // visited is false until algorithim moves to it
        visited = false;

        // used by Wilson's algorithim to determine path
        nextCell = null;
    }

    // gets rid of a wall, returns success or failure (wall doesn't exist or bad wallName given)
    public bool destroyWall(Directions direction)
    {
        switch (direction)
        {
            case Directions.LEFT:
                if (leftWall == null) { return false; }
                GameObject.Destroy(leftWall);
                leftWall = null;
                break;
            case Directions.RIGHT:
                if (rightWall == null) { return false; }
                GameObject.Destroy(rightWall);
                rightWall = null;
                break;
            case Directions.FRONT:
                if (frontWall == null) { return false; }
                GameObject.Destroy(frontWall);
                frontWall= null;
                break;
            case Directions.BACK:
                if (backWall == null) { return false; }
                GameObject.Destroy(backWall);
                backWall = null;
                break;
            default:
                return false;
        }

        return true;
    }


    // test
    public bool destroyWall(string cmd)
    {
        switch (cmd)
        {
            case "all":
                destroyWall(Directions.LEFT);
                destroyWall(Directions.RIGHT);
                destroyWall(Directions.FRONT);
                destroyWall(Directions.BACK);
                return true;
            default:
                Debug.Log("No valid command sent to destroyWall");
                return false;
        }
    }

    public void floorless()
    {
        GameObject.Destroy(floor);
    }

    // gets any and all references to neighbors
    public List<Directions> getNeighbors()
    {
        List<Directions> tiles= new List<Directions>();
        if(frontNeighbor != null) { tiles.Add(Directions.FRONT); }
        if(backNeighbor != null) { tiles.Add(Directions.BACK); }
        if(rightNeighbor != null) { tiles.Add(Directions.RIGHT);}
        if(leftNeighbor != null) { tiles.Add(Directions.LEFT);}

        return tiles;
    }

    // helper function to quickly obtain list of visitable neighbors
    public List<Directions> getUnvisitedNeighbors()
    {
        List<Directions> names = new List<Directions>();

        if (frontNeighbor != null && frontNeighbor.visited != true)
            names.Add(Directions.FRONT);
        if (backNeighbor != null && backNeighbor.visited != true)
            names.Add(Directions.BACK);
        if (leftNeighbor != null && leftNeighbor.visited != true)
            names.Add(Directions.LEFT);
        if (rightNeighbor != null && rightNeighbor.visited != true)
            names.Add(Directions.RIGHT);

        return names;
    }

    //prep for nav mesh
    public bool setNavigationStatic()
    {
        List<GameObject> list = new List<GameObject> { leftWall, rightWall, backWall, frontWall, pillarBR, pillarFR, pillarBL, pillarFL, floor };

        foreach (var item in list)
        {
            if (item != null)
            {
                GameObjectUtility.SetStaticEditorFlags(item, StaticEditorFlags.NavigationStatic);
            }
        }

        return true;
    }

    // gets valid directions (no wall blocking)
    public List<Directions?> getTraversible()
    {
        List<Directions?> dirs = new List<Directions?>();

        if (leftWall == null)
            dirs.Add(Directions.LEFT);
        if (rightWall == null)
            dirs.Add(Directions.RIGHT);
        if (backWall == null)
            dirs.Add(Directions.BACK);
        if (frontWall == null)
            dirs.Add(Directions.FRONT);

        return dirs;
    }

}

public class GridGen : MonoBehaviour
{
    public GameObject gridTile;
    public NavMeshSurface surface;
    public NavMeshAgent enemy;
    public GameObject player;
    
    // Size of grid
    public static int length = 15;
    public static int width = 15;

    // -3.7 is ground level for some reason
    public float height = (float)-3.7;

    // used to randomly pick position
    static System.Random random = new System.Random();

    private int randx = random.Next(0, length);
    private int randy = random.Next(0, width);


    private Tile[,] grid = new Tile[length, width];
    private List<Tile> cellList = new List<Tile>();

    private Tile startPoint, endPoint;
    private Directions startSide, endSide;

    // Start is called before the first frame update
    void Start()
    {
        // i is x axis, j is z axis
        // generate grid
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Based on current prefab, generate a (10x10 by default) grid to make a maze from
                GameObject temp = Instantiate(gridTile, new Vector3(i * 9, height, j * 9), Quaternion.identity);
                //temp.transform.Rotate(new Vector3(0, 90, 0));

                temp.transform.parent = gameObject.transform;

                Tile tile = new Tile(temp);

                grid[i, j] = tile;
                cellList.Add(tile);
                tile.coord = i + "," + j;
            }
        }

        // give each tile references to their neighbors
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                /* if j or i is equal to width or length, this means that tile is an edge
                 * and therefore won't have a right or front (upper) neighbor
                 */
                if (j < width - 1)
                {
                    grid[i, j].frontNeighbor = grid[i, j + 1];
                    grid[i, j + 1].backNeighbor = grid[i, j];
                }
                if (i < length - 1)
                {
                    grid[i, j].rightNeighbor = grid[i + 1, j];
                    grid[i + 1, j].leftNeighbor = grid[i, j];
                }

            }

        }



        // two methods of maze generation, one uses Recursive Backtracking, the other uses Wilson's Algorithim
        generateMaze(randx, randy, grid);
        //generateWilsonMaze(randx, randy, grid);
        int randomStart, randomEnd;
        // next decide start and end, remove appropriate walls

        /*
         * This isn't intuitive at all, but because the grid generates with the cells rotated
         * 90 degrees, it screws with the maze algorithims. This works, but you have to think of 
         * front/back/left/right as sides of the grid and not sides of the cell.
         */

        switch (random.Next(0, 4))
        {
            // front start
            case 0:
                randomStart = random.Next(0, width);
                randomEnd = random.Next(0, width);

                startPoint = grid[length - 1, randomStart];
                startPoint.destroyWall(Directions.RIGHT);

                endPoint = grid[0, randomEnd];
                endPoint.destroyWall(Directions.LEFT);

                startSide = Directions.FRONT;
                endSide = Directions.BACK;
                break;
            // back start
            case 1:
                randomStart = random.Next(0, width);
                randomEnd = random.Next(0, width);

                startPoint = grid[0, randomStart];
                startPoint.destroyWall(Directions.LEFT);

                endPoint = grid[length - 1, randomEnd];
                endPoint.destroyWall(Directions.RIGHT);

                startSide = Directions.BACK;
                endSide = Directions.FRONT;
                break;
            // left start
            case 2:
                randomStart = random.Next(0, width);
                randomEnd = random.Next(0, width);

                startPoint = grid[randomStart, 0];
                startPoint.destroyWall(Directions.BACK);

                endPoint = grid[randomEnd, width - 1];
                endPoint.destroyWall(Directions.FRONT);

                startSide = Directions.LEFT;
                endSide = Directions.RIGHT;
                break;

            // right start
            case 3:
                randomStart = random.Next(0, width);
                randomEnd = random.Next(0, width);

                startPoint = grid[randomStart, width - 1];
                startPoint.destroyWall(Directions.FRONT);

                endPoint = grid[randomEnd, 0];
                endPoint.destroyWall(Directions.BACK);

                startSide = Directions.RIGHT;
                endSide = Directions.LEFT;
                break;
        }

        Instantiate(player, startPoint.floor.transform);
        //rightFollower();
    }

    

    /*private void leftFollower(Tile begin) 
    { 
    
    }*/

    // Wilson's Algorithim
    private void generateWilsonMaze(int x, int y, Tile[,] grid)
    {
        bool done;

        grid[x, y].visited = true;
        Tile start = null;
        // unvisited tiles
        cellList.Remove(grid[x, y]);

        // start at a random node that isn't visited
        while (cellList.Count > 0)
        {
            Tile current = cellList.ElementAt(random.Next(0, cellList.Count));
            start = current;
            Tile next = null;

            done = false;
            while (!done)
            {
                // what to do when randomly finding a visited cell
                if (current.visited)
                {
                    current = start;

                    while (current.nextCell != null)
                    {
                        switch (current.nextCell.Item2)
                        {
                            case Directions.FRONT:
                                current.destroyWall(Directions.FRONT);
                                current.nextCell.Item1.destroyWall(Directions.BACK);
                                break;
                            case Directions.BACK:
                                current.destroyWall(Directions.BACK);
                                current.nextCell.Item1.destroyWall(Directions.FRONT);
                                break;
                            case Directions.RIGHT:
                                current.destroyWall(Directions.RIGHT);
                                current.nextCell.Item1.destroyWall(Directions.LEFT);
                                break;
                            case Directions.LEFT:
                                current.destroyWall(Directions.LEFT);
                                current.nextCell.Item1.destroyWall(Directions.RIGHT);
                                break;
                        }

                        // for efficiency of space, use next as temp reference to clear out current.nextCell
                        // this is important so that after the first iteration of the algorithim, current.nextCell will be null
                        cellList.Remove(current);
                        current.visited = true;
                        next = current.nextCell.Item1;
                        current.nextCell = null;
                        current = next;
                    }
                    done = true;
                }

                else
                {
                    // randomly choose a direction to go in, set as next
                    switch (current.getNeighbors().ElementAt(random.Next(0, current.getNeighbors().Count())))
                    {
                        case Directions.FRONT:
                            next = current.frontNeighbor;
                            current.nextCell = new Tuple<Tile, Directions?>(next, Directions.FRONT);
                            break;
                        case Directions.BACK:
                            next = current.backNeighbor;
                            current.nextCell = new Tuple<Tile, Directions?>(next, Directions.BACK);
                            break;
                        case Directions.LEFT:
                            next = current.leftNeighbor;
                            current.nextCell = new Tuple<Tile, Directions?>(next, Directions.LEFT);
                            break;
                        case Directions.RIGHT:
                            next = current.rightNeighbor;
                            current.nextCell = new Tuple<Tile, Directions?>(next, Directions.RIGHT);
                            break;
                    }

                    current = next;
                }
            }
        }


    }



    // Recursive Backtracking
    List<Directions> travel = new List<Directions>();
    Stack<pair> visits = new Stack<pair>();
    private void generateMaze(int x, int y, Tile[,] grid)
    {
        grid[x,y].visited = true;
        pair p;
        p.y = y; 
        p.x = x;
        // two cases:

        // either there are adjacent nodes/tiles that can be visited, therefore pick one
        travel = grid[x, y].getUnvisitedNeighbors();
        if (travel.Count > 0)
        {
            visits.Push(p);
            // pick random direction (right, left, front, back)
            Directions direction = travel.ElementAt(random.Next(travel.Count));
            switch (direction)
            {
                case Directions.FRONT:
                    grid[x, y].destroyWall(Directions.FRONT);
                    grid[x, y + 1].destroyWall(Directions.BACK);
                    generateMaze(x, y + 1, grid);
                    break;
                case Directions.BACK:
                    grid[x, y].destroyWall(Directions.BACK);
                    grid[x, y - 1].destroyWall(Directions.FRONT);
                    generateMaze(x, y - 1, grid);
                    break;
                case Directions.RIGHT:
                    grid[x, y].destroyWall(Directions.RIGHT);
                    grid[x + 1, y].destroyWall(Directions.LEFT);
                    generateMaze(x + 1, y, grid);
                    break;
                case Directions.LEFT:
                    grid[x, y].destroyWall(Directions.LEFT);
                    grid[x - 1, y].destroyWall(Directions.RIGHT);
                    generateMaze(x - 1, y, grid);
                    break;
                default:
                    Debug.LogError("Invalid direction generated in generateMaze.");
                    break;
            }
        }
        else
        {
            visits.Pop();
            if (visits.Count > 0)
                generateMaze(visits.Peek().x, visits.Peek().y, grid);
        }

    }

    // Update is called once per frame
    private bool done;
    void Update()
    {
        if (done == false)
        {
            done = true;
            surface.BuildNavMesh();
            Instantiate(enemy, new Vector3(randx, height, randy), Quaternion.identity);
        } 

        
    }
}

public struct pair
{
    public int x, y;

    pair(int x, int y)
    {
        this.x= x; 
        this.y = y;
    }
}
