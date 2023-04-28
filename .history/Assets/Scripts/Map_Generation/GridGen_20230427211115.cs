using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
  public bool edgeTile { get; set; }
  public bool rightVisit { get; set; } = false;
  public bool leftVisit { get; set; } = false;

  public string coord { get; set; } = "";

  public GameObject floor { get; set; }

  // reference to previous Tile (used for Wilson's algorithim) - eliminates need for stack
  public Tuple<Tile, Directions?> nextCell { get; set; }

  private GameObject textObject;


  public Tile(GameObject self)
  {
    // need reference to game object this tile corresponds to
    this.self = self;

    // get references to each piece of prefab
    leftWall = self.transform.Find("WallTileLeft").gameObject;
    rightWall = self.transform.Find("WallTileRight").gameObject;
    frontWall = self.transform.Find("WallTileFront").gameObject;
    backWall = self.transform.Find("WallTileBack").gameObject;
    floor = self.transform.Find("TileFloor").gameObject;
    pillarBL = self.transform.Find("BackLeftPillar").gameObject;
    pillarBR = self.transform.Find("BackRightPillar").gameObject;
    pillarFL = self.transform.Find("FrontLeftPillar").gameObject;
    pillarFR = self.transform.Find("FrontRightPillar").gameObject;


    textObject = floor.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

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

    //textObject.GetComponent<TMPro.TextMeshProUGUI>().text = i + ", " + j;
  }

  public void setText(string s)
  {
    textObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
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

        // get rid of pillar too if necessary
        if (backWall == null)
        {
          GameObject.Destroy(pillarBL);
          pillarBL = null;
        }
        else if (frontWall == null)
        {
          GameObject.Destroy(pillarFL);
          pillarFL = null;
        }

        break;
      case Directions.RIGHT:
        if (rightWall == null) { return false; }
        GameObject.Destroy(rightWall);
        rightWall = null;

        if (backWall == null)
        {
          GameObject.Destroy(pillarBR);
          pillarBR = null;
        }
        else if (frontWall == null)
        {
          GameObject.Destroy(pillarFR);
          pillarFR = null;
        }

        break;
      case Directions.FRONT:
        if (frontWall == null) { return false; }
        GameObject.Destroy(frontWall);
        frontWall = null;

        if (leftWall == null)
        {
          GameObject.Destroy(pillarFL);
          pillarFL = null;
        }
        else if (rightWall == null)
        {
          GameObject.Destroy(pillarFR);
          pillarFR = null;
        }

        break;
      case Directions.BACK:
        if (backWall == null) { return false; }
        GameObject.Destroy(backWall);
        backWall = null;

        if (leftWall == null)
        {
          GameObject.Destroy(pillarBL);
          pillarBL = null;
        }
        else if (rightWall == null)
        {
          GameObject.Destroy(pillarBR);
          pillarBR = null;
        }
        break;
      default:
        return false;
    }

    return true;
  }

  public void floorless()
  {
    if (floor != null)
    {
      GameObject.Destroy(floor);
      floor = null;
    }
  }

  public void pillarCheck()
  {
    if (frontWall == null && rightWall == null)
      GameObject.Destroy(pillarFR);
    if (frontWall == null && leftWall == null)
      GameObject.Destroy(pillarFL);
    if (backWall == null && rightWall == null)
      GameObject.Destroy(pillarBR);
    if (backWall == null && leftWall == null)
      GameObject.Destroy(pillarBL);
  }

  // gets any and all references to neighbors
  public List<Directions> getNeighbors()
  {
    List<Directions> tiles = new List<Directions>();
    if (frontNeighbor != null) { tiles.Add(Directions.FRONT); }
    if (backNeighbor != null) { tiles.Add(Directions.BACK); }
    if (rightNeighbor != null) { tiles.Add(Directions.RIGHT); }
    if (leftNeighbor != null) { tiles.Add(Directions.LEFT); }

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

  public List<Directions> getWalls()
  {
    List<Directions> walls = new List<Directions>();

    if (leftWall != null)
      walls.Add(Directions.LEFT);
    if (rightWall != null)
      walls.Add(Directions.RIGHT);
    if (frontWall != null)
      walls.Add(Directions.FRONT);
    if (backWall != null)
      walls.Add(Directions.BACK);

    return walls;
  }

}

public class GridGen : MonoBehaviour
{
  public GameObject gridTile;
  public NavMeshSurface surface;
  public NavMeshAgent enemy;
  public GameObject player;
  public GameObject psx;
  //public int EditorLength;
  //public int EditorWidth;


  // Size of grid
  public static int length = 4;
  public static int width = 4;

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

  public Tile[,] getGrid()
  {
    return grid;
  }

  // Start is called before the first frame update
  void Start()
  {
    //    if (EditorLength != null)
    //        length = EditorLength;
    //    if (EditorWidth != null)
    //        width = EditorWidth;

    // i is x axis, j is z axis
    // generate grid
    for (int i = 0; i < length; i++)
    {
      for (int j = 0; j < width; j++)
      {
        // Based on current prefab, generate a (10x10 by default) grid to make a maze from
        GameObject temp = Instantiate(gridTile, new Vector3(i * -9.5f, height, j * 9.5f), Quaternion.identity);
        //temp.transform.Rotate(new Vector3(0, 90, 0));


        temp.transform.parent = gameObject.transform;

        Tile tile = new Tile(temp);
        //tile.setText(i + "," + j);

        tile.coord = i + " " + j;

        grid[i, j] = tile;
        cellList.Add(tile);

        if (i == 0 || j == 0 || i == length - 1 || j == width - 1)
        {
          tile.edgeTile = true;
        }
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
          grid[i, j].rightNeighbor = grid[i, j + 1];
          grid[i, j + 1].leftNeighbor = grid[i, j];
        }
        if (i < length - 1)
        {
          grid[i, j].frontNeighbor = grid[i + 1, j];
          grid[i + 1, j].backNeighbor = grid[i, j];
        }



      }

    }



    // two methods of maze generation, one uses Recursive Backtracking, the other uses Wilson's Algorithim
    //generateMaze(randx, randy, grid);
    generateWilsonMaze(randx, randy, grid);
    int randomStart, randomEnd;
    // next decide start and end, remove appropriate walls


    // randomly pick 2 edge tiles to start and end on
    switch (random.Next(0, 4))
    {
      // front start
      case 0:
        Debug.Log("front");
        randomStart = random.Next(0, width);
        randomEnd = random.Next(0, width);

        startPoint = grid[length - 1, randomStart];
        startPoint.destroyWall(Directions.FRONT);

        endPoint = grid[0, randomEnd];
        endPoint.destroyWall(Directions.BACK);

        startSide = Directions.FRONT;
        endSide = Directions.BACK;
        break;
      // back start
      case 1:
        Debug.Log("back");
        randomStart = random.Next(0, width);
        randomEnd = random.Next(0, width);

        startPoint = grid[0, randomStart];
        startPoint.destroyWall(Directions.BACK);

        endPoint = grid[length - 1, randomEnd];
        endPoint.destroyWall(Directions.FRONT);

        startSide = Directions.BACK;
        endSide = Directions.FRONT;
        break;
      // left start
      case 2:
        Debug.Log("left");
        randomStart = random.Next(0, length);
        randomEnd = random.Next(0, length);

        startPoint = grid[randomStart, 0];
        startPoint.destroyWall(Directions.LEFT);

        endPoint = grid[randomEnd, width - 1];
        endPoint.destroyWall(Directions.RIGHT);

        startSide = Directions.LEFT;
        endSide = Directions.RIGHT;
        break;

      // right start
      case 3:
        Debug.Log("right");
        randomStart = random.Next(0, length);
        randomEnd = random.Next(0, length);

        startPoint = grid[randomStart, width - 1];
        startPoint.destroyWall(Directions.RIGHT);

        endPoint = grid[randomEnd, 0];
        endPoint.destroyWall(Directions.LEFT);

        startSide = Directions.RIGHT;
        endSide = Directions.LEFT;
        break;
    }

    Instantiate(player, startPoint.floor.transform);

    // runs right and left wall follower algorithims, then deletes random walls on the correct path to create loops
    deBottleneck();

    // remove floating pillars
    for (int j = 0; j < width; j++)
    {
      for (int i = 0; i < length; i++)
      {
        grid[i, j].pillarCheck();
      }
    }
  }

  // for debugging
  private string dString(Directions d)
  {
    switch (d)
    {
      case Directions.FRONT:
        return "Front";
      case Directions.BACK:
        return "Back";
      case Directions.LEFT:
        return "Left";
      case Directions.RIGHT:
        return "Right";
    }

    Debug.LogError("Bad direction in dString. Should be impossible??");
    return "what???";
  }


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
    grid[x, y].visited = true;
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
          grid[x + 1, y].destroyWall(Directions.BACK);
          generateMaze(x + 1, y, grid);
          break;
        case Directions.BACK:
          grid[x, y].destroyWall(Directions.BACK);
          grid[x - 1, y].destroyWall(Directions.FRONT);
          generateMaze(x - 1, y, grid);
          break;
        case Directions.RIGHT:
          grid[x, y].destroyWall(Directions.RIGHT);
          grid[x, y + 1].destroyWall(Directions.LEFT);
          generateMaze(x, y + 1, grid);
          break;
        case Directions.LEFT:
          grid[x, y].destroyWall(Directions.LEFT);
          grid[x, y - 1].destroyWall(Directions.RIGHT);
          generateMaze(x, y - 1, grid);
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
  private void rightFollower()
  {
    Directions? right = null;
    Tile current = startPoint;

    switch (startSide)
    {
      case Directions.FRONT:
        right = Directions.LEFT;
        break;
      case Directions.BACK:
        right = Directions.RIGHT;
        break;
      case Directions.LEFT:
        right = Directions.BACK;
        break;
      case Directions.RIGHT:
        right = Directions.FRONT;
        break;
    }

    while (current != endPoint)
    {
      current.rightVisit = true;
      // first, attempt to go right
      if (current.getTraversible().Contains(right))
      {
        switch (right)
        {
          case Directions.FRONT:
            right = Directions.RIGHT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            right = Directions.LEFT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            right = Directions.FRONT;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            right = Directions.BACK;
            current = current.rightNeighbor;
            break;
        }
      }
      // attempt to go up/front next
      else if (current.getTraversible().Contains(getCounterClockwise(right)))
      {
        switch (getCounterClockwise(right))
        {
          case Directions.FRONT:
            right = Directions.RIGHT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            right = Directions.LEFT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            right = Directions.FRONT;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            right = Directions.BACK;
            current = current.rightNeighbor;
            break;
        }
      }
      // then attempt to go left
      else if (current.getTraversible().Contains(getClockwise(getClockwise(right))))
      {
        switch (getClockwise(getClockwise(right)))
        {
          case Directions.FRONT:
            right = Directions.RIGHT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            right = Directions.LEFT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            right = Directions.FRONT;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            right = Directions.BACK;
            current = current.rightNeighbor;
            break;
        }
      }
      // only other option is back
      else
      {
        switch (getClockwise(right))
        {
          case Directions.FRONT:
            right = Directions.RIGHT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            right = Directions.LEFT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            right = Directions.FRONT;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            right = Directions.BACK;
            current = current.rightNeighbor;
            break;
        }
      }
    }
  }

  private void leftFollower()
  {
    Directions? left = null;
    Tile current = startPoint;

    switch (startSide)
    {
      case Directions.FRONT:
        left = Directions.RIGHT;
        break;
      case Directions.BACK:
        left = Directions.LEFT;
        break;
      case Directions.LEFT:
        left = Directions.FRONT;
        break;
      case Directions.RIGHT:
        left = Directions.BACK;
        break;
    }

    while (current != endPoint)
    {
      current.leftVisit = true;

      if (current.leftVisit == true && current.rightVisit == true)
      {
        //current.setText("PATH");
        if (!current.edgeTile)
          correctPath.Add(current);
      }

      // first, attempt to go left
      if (current.getTraversible().Contains(left))
      {
        switch (left)
        {
          case Directions.FRONT:
            left = Directions.LEFT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            left = Directions.RIGHT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            left = Directions.BACK;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            left = Directions.FRONT;
            current = current.rightNeighbor;
            break;
        }
      }
      // attempt to go up/front next
      else if (current.getTraversible().Contains(getClockwise(left)))
      {
        switch (getClockwise(left))
        {
          case Directions.FRONT:
            left = Directions.LEFT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            left = Directions.RIGHT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            left = Directions.BACK;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            left = Directions.FRONT;
            current = current.rightNeighbor;
            break;
        }
      }
      // then attempt to go right
      else if (current.getTraversible().Contains(getClockwise(getClockwise(left))))
      {
        switch (getClockwise(getClockwise(left)))
        {
          case Directions.FRONT:
            left = Directions.LEFT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            left = Directions.RIGHT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            left = Directions.BACK;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            left = Directions.FRONT;
            current = current.rightNeighbor;
            break;
        }
      }
      // only other option is back
      else
      {
        switch (getCounterClockwise(left))
        {
          case Directions.FRONT:
            left = Directions.LEFT;
            current = current.frontNeighbor;
            break;
          case Directions.BACK:
            left = Directions.RIGHT;
            current = current.backNeighbor;
            break;
          case Directions.LEFT:
            left = Directions.BACK;
            current = current.leftNeighbor;
            break;
          case Directions.RIGHT:
            left = Directions.FRONT;
            current = current.rightNeighbor;
            break;
        }
      }
    }
  }

  private Directions? getClockwise(Directions? d)
  {
    switch (d)
    {
      case Directions.FRONT:
        return Directions.RIGHT;
      case Directions.BACK:
        return Directions.LEFT;
      case Directions.LEFT:
        return Directions.FRONT;
      case Directions.RIGHT:
        return Directions.BACK;
      default:
        Debug.LogError("Bad direction in getClockwise");
        return null;
    }
  }

  private Directions? getCounterClockwise(Directions? d)
  {
    switch (d)
    {
      case Directions.FRONT:
        return Directions.LEFT;
      case Directions.BACK:
        return Directions.RIGHT;
      case Directions.LEFT:
        return Directions.BACK;
      case Directions.RIGHT:
        return Directions.FRONT;
      default:
        Debug.LogError("Bad direction in getCounterClockwise");
        return null;
    }
  }

  List<Tile> correctPath = new List<Tile>();

  private void deBottleneck()
  {
    rightFollower();
    leftFollower();

    // edit 1/3rd of the tiles on the correct path
    int deleteTiles = 0; // correctPath.Count() / 3;
    int chosenTile;
    Tile current;
    for (int i = 0; i < deleteTiles && correctPath.Count() > 0; i++)
    {
      // randomly pick a tile from the tile that make up the correct/most efficient path
      // this doesn't include edge tiles
      chosenTile = random.Next(0, correctPath.Count());
      current = correctPath[chosenTile];
      //current.setText("EDIT");

      // now, pick a random available wall from the chosen tile to delete
      // ensure picked tile has walls available
      if (current.getWalls().Count() > 0)
      {
        switch (current.getWalls().ElementAt(random.Next(0, current.getWalls().Count())))
        {
          case Directions.FRONT:
            current.destroyWall(Directions.FRONT);
            current.frontNeighbor.destroyWall(Directions.BACK);
            break;
          case Directions.BACK:
            current.destroyWall(Directions.BACK);
            current.backNeighbor.destroyWall(Directions.FRONT);
            break;
          case Directions.LEFT:
            current.destroyWall(Directions.LEFT);
            current.leftNeighbor.destroyWall(Directions.RIGHT);
            break;
          case Directions.RIGHT:
            current.destroyWall(Directions.RIGHT);
            current.rightNeighbor.destroyWall(Directions.LEFT);
            break;
        }
      }

      // don't allow same tile to be edited twice
      correctPath.RemoveAt(chosenTile);
    }
  }

  // Update is called once per frame
  private bool done;
  private ClickController enemyLocation;
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
    this.x = x;
    this.y = y;
  }
}
