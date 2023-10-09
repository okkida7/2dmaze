using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject mazeCellPrefab; 
    public float cellSize = 1f;
    private Vector2Int doorPosition;

    private void Awake()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();
        
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(mazeCellPrefab, new Vector3((float)x * cellSize, (float)y * cellSize, 0f), Quaternion.identity);
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                bool right = false;
                bool bottom = false;

                doorPosition = new Vector2Int((int)maze[x,y].position.x, (int)maze[x,y].position.y);
                
                if(x == mazeGenerator.mazeWidth - 1) right = maze[(int)doorPosition.x, (int)doorPosition.y].rightWall;
                if(y == 0) bottom = maze[(int)doorPosition.x, (int)doorPosition.y].bottomWall;

                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
}
