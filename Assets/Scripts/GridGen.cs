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

public class Tile
{
    GameObject self;
    
    GameObject leftWall, rightWall, backWall, frontWall, pillarBR, pillarFR, pillarBL, pillarFL, floor;

    public Tile frontNeighbor { get; set; }
    public Tile backNeighbor { get; set; }
    public Tile rightNeighbor { get; set; }
    public Tile leftNeighbor { get; set; }

    public bool visited { get; set; }

    public Tile(GameObject self)
    {
        // need reference to game object this tile corresponds to
        this.self = self;

        // prefab will always have walls
        leftWall = self.transform.Find("WallTileLeft").gameObject;
        //Debug.Log(leftWall.name);
        rightWall = self.transform.Find("WallTileRight").gameObject;
        //Debug.Log(rightWall.name);
        frontWall = self.transform.Find("WallTileFront").gameObject;
        //Debug.Log(frontWall.name);
        backWall = self.transform.Find("WallTileBack").gameObject;
        //Debug.Log(backWall.name);
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
    }

    // gets rid of a wall, returns success or failure (wall doesn't exist or bad wallName given)
    public bool destroyWall(string wallName)
    {
        switch (wallName)
        {
            case "left":
                if (leftWall == null) { return false; }
                GameObject.Destroy(leftWall);
                leftWall = null;
                break;
            case "right":
                if (rightWall == null) { return false; }
                GameObject.Destroy(rightWall);
                rightWall = null;
                break;
            case "front":
                if (frontWall == null) { return false; }
                GameObject.Destroy(frontWall);
                frontWall= null;
                break;
            case "back":
                if (backWall == null) { return false; }
                GameObject.Destroy(backWall);
                backWall = null;
                break;
            default:
                return false;
        }

        return true;
    }

    // helper function to quickly obtain list of visitable neighbors
    public List<string> getUnvisitedNeighbors()
    {
        List<string> names = new List<string>();

        if (frontNeighbor != null && frontNeighbor.visited != true)
            names.Add("front");
        if (backNeighbor != null && backNeighbor.visited != true)
            names.Add("back");
        if (leftNeighbor != null && leftNeighbor.visited != true)
            names.Add("left");
        if (rightNeighbor != null && rightNeighbor.visited != true)
            names.Add("right");

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
}

public class GridGen : MonoBehaviour
{
    public GameObject gridTile;
    public NavMeshSurface surface;
    public NavMeshAgent enemy;
    
    // Leaving grid size settable, since maybe this would be linked to difficulty
    public static int length = 10;
    public static int width = 10;
    public float height = (float)-3.7;

    private bool done = false;

    static System.Random random = new System.Random();

    private int randx = random.Next(0, length);
    private int randy = random.Next(0, width);


    private Tile[,] grid = new Tile[length, width];

    

    // Start is called before the first frame update
    void Start()
    {
        // i is x axis, j is z axis
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Based on current prefab, generate a (10x10 by default) grid to make a maze from
                GameObject temp = Instantiate(gridTile, new Vector3(i * 9, height, j * 9), Quaternion.identity);
                temp.transform.parent = gameObject.transform;

                Tile tile = new Tile(temp);
                
                grid[i, j] = tile;
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

        
        generateMaze(randx, randy, grid);

    }

    
    List<string> travel = new List<string>();
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
            string direction = travel.ElementAt(random.Next(travel.Count));
            switch (direction)
            {
                case "front":
                    grid[x, y].destroyWall("front");
                    grid[x, y + 1].destroyWall("back");
                    generateMaze(x, y + 1, grid);
                    break;
                case "back":
                    grid[x, y].destroyWall("back");
                    grid[x, y - 1].destroyWall("front");
                    generateMaze(x, y - 1, grid);
                    break;
                case "right":
                    grid[x, y].destroyWall("right");
                    grid[x + 1, y].destroyWall("left");
                    generateMaze(x + 1, y, grid);
                    break;
                case "left":
                    grid[x, y].destroyWall("left");
                    grid[x - 1, y].destroyWall("right");
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
