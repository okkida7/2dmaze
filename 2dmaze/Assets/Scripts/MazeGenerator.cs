using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 100)]
    public int mazeWidth = 5, mazeHeight = 5;
    public int startX, startY;
    MazeCell[,] maze;
    Vector2Int currentCell;
    List<Vector2Int> deadEnds = new List<Vector2Int>();
    public Vector2Int doorPosition;
    
    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }
        CarvePath(startX, startY);
        PlaceDoorAtRandomEdge(new Vector3(0,0,0));
        return maze;
    }

    List<Direction> directions = new List<Direction>
    {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };

        List<Direction> GetRandomDirections()
    {
        List<Direction> dir = new List<Direction>(directions);
        List<Direction> rndDir = new List<Direction>();

        while(dir.Count > 0)
        {
            int rnd = Random.Range(0,dir.Count);
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }
        return rndDir;
    }
    bool IsCellValid(int x, int y)
    {
        if(x < 0 || x >= mazeWidth || y < 0 || y >= mazeHeight || maze[x,y].visited)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    Vector2Int CheckNeighbour()
    {
        List<Direction> rndDir = GetRandomDirections();
        for(int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell;
            switch(rndDir[i])
            {
                case Direction.Up:
                    neighbour.y += 1;
                    break;
                case Direction.Down:
                    neighbour.y -= 1;
                    break;
                case Direction.Left:
                    neighbour.x -= 1;
                    break;
                case Direction.Right:
                    neighbour.x += 1;
                    break;
            }
            if(IsCellValid(neighbour.x, neighbour.y))
            {
                return neighbour;
            }
        }
        return currentCell;
    }
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if(primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }else if(primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }else if(primaryCell.y < secondaryCell.y)
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }else if(primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }
    void CarvePath(int x, int y)
    {
        if(x < 0 || y < 0 || x >= mazeWidth || y >= mazeHeight)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds. Resetting to (0,0)");
        }
        currentCell = new Vector2Int(x,y);
        List<Vector2Int> path = new List<Vector2Int>();
        bool deadEnd = false;
        while(!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbour();
            if(nextCell == currentCell)
            {
                deadEnds.Add(currentCell);
                for(int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbour();
                    if(nextCell != currentCell)
                    {
                        break;
                    }
                }
                if(nextCell == currentCell)
                {
                    deadEnd = true;
                }
            }else{
                BreakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].visited = true;
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
        
    }
    float CalculateDistance(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointA, pointB);
    }

    List<Vector3> GetFarEdgeCells(Vector3 playerPosition, float threshold)
    {
        List<Vector3> edgeCells = new List<Vector3>();
        List<Vector3> farCells = new List<Vector3>();
        for(int x = 0; x < mazeWidth; x++)
        {
            edgeCells.Add(new Vector3(x, 0, 0));
            edgeCells.Add(new Vector3(x, 0, mazeHeight - 1));
        }

        // Collect left and right edge cells.
        for(int y = 0; y < mazeHeight; y++)
        {
            edgeCells.Add(new Vector3(0, 0, y));
            edgeCells.Add(new Vector3(mazeWidth - 1, 0, y));
        }

        // Filter out cells that are too close to the player.
        foreach (Vector3 cell in edgeCells)
        {
            if(CalculateDistance(playerPosition, cell) >= threshold)
            {
                farCells.Add(cell);
            }
        }

        return farCells;
    }

        void PlaceDoorAtRandomEdge(Vector3 playerPosition)
    {
        float distanceThreshold = Mathf.Sqrt(mazeWidth * mazeWidth + mazeHeight * mazeHeight)/2f;
        List<Vector3> farCells = GetFarEdgeCells(playerPosition, distanceThreshold);

        if(farCells.Count == 0)
        {
            Debug.LogError("No suitable location found for the door. Increase the maze size or reduce the threshold.");
            return;
        }

        Vector3 doorPosition = farCells[Random.Range(0, farCells.Count)];

        // Adjusting the door's position, rotation, and wall data based on the edge.
        Quaternion doorRotation = Quaternion.identity; 

        maze[(int)doorPosition.x, (int)doorPosition.y].leftWall = false;
        maze[(int)doorPosition.x, (int)doorPosition.y].rightWall = false;
        maze[(int)doorPosition.x, (int)doorPosition.y].bottomWall = false;
        maze[(int)doorPosition.x, (int)doorPosition.y].topWall = false;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (current.x > 0 && !maze[current.x, current.y].leftWall)
            neighbors.Add(new Vector2Int(current.x - 1, current.y));

        if (current.x < mazeWidth - 1 && !maze[current.x, current.y].rightWall)
            neighbors.Add(new Vector2Int(current.x + 1, current.y));

        if (current.y > 0 && !maze[current.x, current.y].topWall)
            neighbors.Add(new Vector2Int(current.x, current.y - 1));

        if (current.y < mazeHeight - 1 && !maze[current.x, current.y].bottomWall)
            neighbors.Add(new Vector2Int(current.x, current.y + 1));

        return neighbors;
    }


    void Start()
    {

    }

    void Update()
    {

    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    public bool visited;
    public int x, y;
    public bool topWall;
    public bool leftWall;
    public bool rightWall;
    public bool bottomWall;

    public Vector2Int position => new Vector2Int(x, y);

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;

        visited = false;
        topWall = leftWall = true;
        rightWall = bottomWall = true;
    }
}
