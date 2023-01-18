using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Handle general game flow
public class GameManager : MonoBehaviour
{
    //Maze Data
    private GameObject[,] mazeGO;
    private string fileName;
    private List<Vector2> solvedPath = new List<Vector2>();

    //Maze GOs, Prefabs & Sprites
    public GameObject MazeBG;
    public GameObject MazePiecePrefab;
    public GameObject MazeMan;
    private MazeManController mazeManController;
    public Sprite MazeJointIMG;
    public Sprite MazeEdgeIMG;
    public Sprite MazeFinishIMG;
    public GameObject ImpossibleText;

    //Maze Population Variables
    private float evenAdjust = .5f; //Offset to subtract to each piece if the maze dimensions on that axis is even
    public MazeSolver mazeSolver;

    //Camera, Audio, & UI Management
    private Camera mainCamera;
    private float initialCameraSize;
    private UIManager uiManager;
    private AudioManager audioManager;

    private void Start(){
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        initialCameraSize = mainCamera.orthographicSize;

        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        mazeManController = MazeMan.GetComponent<MazeManController>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    public void SetMaze(char[,] maze){
        DrawMaze(maze);

        List<Vector2> solvedPath = new List<Vector2>();
        solvedPath = mazeSolver.SolveMaze(maze);

        if (solvedPath.Count > 0){  //Either start walking maze, or have Maze Man shrug if impossible
            MazeMan.GetComponent<MazeManController>().BeginMaze(ConvertPathToCoordinates(solvedPath));
        } else {
            mazeManController.SetAnimationTrigger("FileError");
            audioManager.PrepareError();
            ImpossibleText.SetActive(true);
        }
    }

    private void DrawMaze(char[,] maze){
        uiManager.ToggleMazeMode(true);

        MazeBG.SetActive(true);
        MazeBG.transform.localScale = new Vector3(maze.GetLength(0)+1, maze.GetLength(1)+1);

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
                    mazeGO[x,y].tag = "Finish";
                } else {
                    mazeGO[x,y].tag = "Blank";  //Used for MazeMan Green Dots placement
                }
            }
        }

        MazeMan.transform.position = new Vector3(-2 - xAdjust, -1 + yAdjust, transform.position.z);
    }

    private List<Vector2>ConvertPathToCoordinates(List<Vector2> path){
        List<Vector2> Coordinates = new List<Vector2>();

        for (int i=0; i < path.Count; i++){
            Coordinates.Add(mazeGO[(int)path[i].x, (int)path[i].y].transform.position);
        }

        return Coordinates;
    }

    public void ResetToTitle(){
        uiManager.ToggleMazeMode(false);
        mazeManController.Restart();

        GameObject.Find("File Reader").GetComponent<FileReader>().ResetFileReader();

        solvedPath.Clear();
        MazeBG.SetActive(false);
        ImpossibleText.SetActive(false);

        audioManager.ResetFootCount();

        //NEED TO OPTOMIZE THIS LATER IF TIME!!! Need to be reusing game objects instead of mass delete & recreate
        for (int x = 0; x < mazeGO.GetLength(0); x++) {
            for (int y = 0; y < mazeGO.GetLength(1); y++){
                Destroy(mazeGO[x,y]);
            }
        }

        mainCamera.orthographicSize = initialCameraSize;
    }
}
