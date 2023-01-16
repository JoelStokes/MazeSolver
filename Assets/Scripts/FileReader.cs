using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;  

public class FileReader : MonoBehaviour
{
    public TMP_InputField inputText;
    private string filePath;
    private string appPath;
    private bool parsing = false;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        appPath = Application.dataPath;
    }

    void Update(){  //Check for Enter key in case it's used instead of clicking Upload button
        if (Input.GetKeyDown(KeyCode.Return) && !parsing){
            CheckPath();
        }
    }

    public void CheckPath(){    //Make sure inputText is a valid filePath before parsing
        parsing = true;

        if (inputText.text == null && inputText.text == ""){    
            NoFileTyped();
        } else {
            if (inputText.text.Contains("/")){  //Check if they input whole file path or if it's just the text file inside the exe folder
                filePath = inputText.text;
            } else {
                filePath = appPath + "/" + inputText.text;
            }

            //Check if input name includes ".txt". If not, add it
            if (!filePath.Contains(".txt")){
                filePath += ".txt";
            }
            
            var sr = new StreamReader(filePath);
            var fileContents = sr.ReadToEnd();
            sr.Close();
        
            ParseFile(fileContents);
        }

    }

    private void ParseFile(string fileContents){    //Split read file into length by height, then populate character array
        var lines = fileContents.Split("\n"[0]);
        char[,] mazeGrid = new char[lines[0].Length-1, lines.Length];   //-1 off x to ignore ending white space on Kodable maze NEEDS TO CHANGE TO BE MORE ROBUST! NOT GOOD SOLUTION

        for (int x = 0; x < lines[0].Length-1; x++) {
            for (int y = 0; y < lines.Length; y++){
                mazeGrid[x,y] = lines[y][x];
            }
        }

        gameManager.SetMaze(mazeGrid);
    }

    private void NoFileTyped(){ 

    }
}