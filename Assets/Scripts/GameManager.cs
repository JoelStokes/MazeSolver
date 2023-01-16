using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Maze Data
    private char[,] maze;
    private string fileName;

    //Audio Management
    public Image audioImage;
    public Sprite audioOnImg;
    public Sprite audioOffImg;
    private bool audioOn = true;

    public void SetMaze(char[,] newMaze){
        maze = newMaze;
        StartSolve();
    }

    private void StartSolve(){

    }

    public void ToggleAudio(){  //Turn background music & sfx on/off
        audioOn = !audioOn;

        if (audioOn){
            audioImage.sprite = audioOnImg;
        } else {
            audioImage.sprite = audioOffImg;
        }
    }
}
