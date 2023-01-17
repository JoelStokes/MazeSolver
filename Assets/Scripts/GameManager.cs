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

    //Maze Prefabs & Sprites
    public GameObject MazeBG;
    public GameObject MazePiece;
    public Sprite MazeJointIMG;
    public Sprite MazeEdgeIMG;

    //Maze Population Variables
    float evenAdjust = .5f; //Offset to subtract to each piece if the maze dimensions on that axis is even

    //Audio Management
    public Image audioImage;
    public Sprite audioOnImg;
    public Sprite audioOffImg;
    private bool audioOn = true;

    //Camera Management
    private Camera mainCamera;
    private float initialCameraSize;

    private void Start(){
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        initialCameraSize = mainCamera.orthographicSize;
    }

    public void SetMaze(char[,] newMaze){
        maze = newMaze;
        //Hide UI Elements?
        StartSolve();
    }

    private void StartSolve(){
        GameObject newMazeBG = Instantiate(MazeBG, transform);
        newMazeBG.transform.localScale = new Vector3(maze.GetLength(0)+1, maze.GetLength(1)+1);

        mainCamera.orthographicSize = (maze.GetLength(0)/2);    //Scale camera to fit maze on screen to help support small to large mazes

        float xAdjust = maze.GetLength(0)/2;
        float yAdjust = maze.GetLength(1)/2;    //Prepare coordinates for Maze Game Objects

        if (maze.GetLength(0) % 2 == 0){
            xAdjust -= evenAdjust;
        } 

        if (maze.GetLength(1) % 2 == 0){
            yAdjust -= evenAdjust;
        }

        mazeGO = new GameObject[maze.GetLength(0), maze.GetLength(1)];  //Convert char array Maze into GameObject Maze
        for (int x = 0; x < maze.GetLength(0); x++) {
            for (int y = 0; y < maze.GetLength(1); y++){
                Vector3 position = new Vector3(x - xAdjust, -y + yAdjust, transform.position.z);
                mazeGO[x,y] = Instantiate(MazePiece, position, MazePiece.transform.rotation); //NEED TO SETUP OBJECT POOLING SYSTEM! This will be a huge waste to create & destroy for each maze

                if (maze[x,y].Equals('+')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeJointIMG;
                } else if (maze[x,y].Equals('-')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeEdgeIMG;
                } else if (maze[x,y].Equals('|')){
                    mazeGO[x,y].GetComponent<SpriteRenderer>().sprite = MazeEdgeIMG;
                    mazeGO[x,y].transform.Rotate(0,0,90);
                }
                //Set sprite based on surrounding pieces
            }
        }
    }

    public void ToggleAudio(){  //Turn background music & sfx on/off
        audioOn = !audioOn;

        if (audioOn){
            audioImage.sprite = audioOnImg;
        } else {
            audioImage.sprite = audioOffImg;
        }
    }

    public void ResetToTitle(){

    }
}
