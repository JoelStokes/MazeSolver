using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Maze Data
    private char[,] maze;
    private GameObject[,] mazeGO;
    private string fileName;
    private bool solving = false;
    private List<Vector2> solvedPath = new List<Vector2>();

    //Maze Prefabs & Sprites
    public GameObject MazeBGPrefab;
    public GameObject MazePiecePrefab;
    public GameObject MazeMan;
    public Sprite MazeJointIMG;
    public Sprite MazeEdgeIMG;
    public Sprite MazeFinishIMG;

    //Maze Population Variables
    float evenAdjust = .5f; //Offset to subtract to each piece if the maze dimensions on that axis is even

    //Camera & UI Management
    private Camera mainCamera;
    private float initialCameraSize;
    private UIManager uiManager;

    private void Start(){
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        initialCameraSize = mainCamera.orthographicSize;

        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }

    public void SetMaze(char[,] newMaze){
        maze = newMaze;
        //Hide UI Elements?
        DrawMaze();
        SolveMaze();
    }

    private void DrawMaze(){
        uiManager.ToggleMazeMode(true);

        GameObject newMazeBG = Instantiate(MazeBGPrefab, transform);
        newMazeBG.transform.localScale = new Vector3(maze.GetLength(0)+1, maze.GetLength(1)+1);

        if (maze.GetLength(0) > maze.GetLength(1)){
            mainCamera.orthographicSize = ((maze.GetLength(0)/2.1f) - 1);    //Scale camera to fit maze on screen to help support tiny to massive mazes
        } else {
            mainCamera.orthographicSize = ((maze.GetLength(1)/2.1f) - 1);
        }

        float xAdjust = maze.GetLength(0)/2;
        float yAdjust = maze.GetLength(1)/2;    //Prepare coordinates for Maze Game Objects

        if (maze.GetLength(0) % 2 == 0){
            xAdjust -= evenAdjust;
        } 

        if (maze.GetLength(1) % 2 == 0){
            yAdjust -= evenAdjust;
        }

        maze[maze.GetLength(0)-1,maze.GetLength(1)-2] = '=';    //Place new Finish Line character

        mazeGO = new GameObject[maze.GetLength(0), maze.GetLength(1)];  //Convert char array Maze into GameObject Maze
        for (int x = 0; x < maze.GetLength(0); x++) {
            for (int y = 0; y < maze.GetLength(1); y++){
                Vector3 position = new Vector3(x - xAdjust, -y + yAdjust, transform.position.z);
                mazeGO[x,y] = Instantiate(MazePiecePrefab, position, MazePiecePrefab.transform.rotation); //NEED TO SETUP OBJECT POOLING SYSTEM! This will be a huge waste to create & destroy for each maze

                if (maze[x,y].Equals('+')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeJointIMG;
                } else if (maze[x,y].Equals('-')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeEdgeIMG;
                } else if (maze[x,y].Equals('|')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeEdgeIMG;
                    mazeGO[x,y].transform.Rotate(0,0,90);
                } else if (maze[x,y].Equals('=')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeFinishIMG;
                } else {
                    mazeGO[x,y].tag = "Blank";  //Used for MazeMan Green Dots placement
                }
            }
        }

        MazeMan.transform.position = new Vector3(-3 - xAdjust, -1 + yAdjust, transform.position.z);
    }

    private void SolveMaze(){
        bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)];   //Visited array ensures no endless loop happens in maze
        bool solvable = RecursiveSearch(0, 1, visited);   //Maze opening always starts at (0,1)

        if (solvable){
            MazeMan.GetComponent<MazeManController>().BeginMaze(solvedPath);
        } else {

            //"Impossible to Solve" dialog with sad man
        }
    }

    private bool RecursiveSearch(int x, int y, bool[,] visited){
        bool finished = false;

        if (maze[x,y] == '='){    //Finished adding to list, exit recursion
            return true;
        }

        visited[x,y] = true;

        //Check each direction to see if empty & non-visited
        if (IsViableSpot(x, y+1, visited))     //North
            finished = RecursiveSearch(x, y+1, visited);
        if (!finished && IsViableSpot(x+1, y, visited))  //East
            finished = RecursiveSearch(x+1, y, visited);
        if (!finished && IsViableSpot(x, y-1, visited))  //South
            finished = RecursiveSearch(x, y-1, visited);
        if (!finished && (x != 0 || y != 1) && IsViableSpot(x-1, y, visited))  //West, extra check to prevent breaking out of maze, .txt has open start
            finished = RecursiveSearch(x-1, y, visited);

        if (finished){  //Add coordinates to List & return to previous recursion
            Vector2 position = mazeGO[x,y].transform.position;
            solvedPath.Insert(0, position);
            return true;
        }
        return false;   //Dead End, loop back to last possible path
    }

    private bool IsViableSpot(int x, int y, bool[,] visited){   //Check if space is empty or finish line, and that this spot has never been visited before
        return ((maze[x,y] == ' ' || maze[x,y] == '=') && !visited[x,y]);
    }

    public void ResetToTitle(){
        uiManager.ToggleMazeMode(false);

        //ADD REST OF THIS
    }
}
