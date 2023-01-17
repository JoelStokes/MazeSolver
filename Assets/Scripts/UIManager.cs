using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Menu Sections
    public GameObject MainMenu;
    public GameObject HelpMenu;
    public GameObject Headers;

    //Fast Forward
    public GameObject FastForwardButton;
    public Image FastForwardImage;
    private bool fastForwardActive = false;
    public Sprite FastForwardIcon;
    public Sprite PlayIcon;

    //Audio Management
    public Image audioImage;
    public Sprite audioOnImg;
    public Sprite audioOffImg;
    private bool audioOn = true;

    void Start(){
        MainMenu.SetActive(true);
        Headers.SetActive(true);
        HelpMenu.SetActive(false);
        FastForwardButton.SetActive(false);
    }

    void Update(){  //Allows closing "Help" menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape)){
            CloseHelp();
        }
    }

    public void OpenHelp(){
        HelpMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void CloseHelp(){
        HelpMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ToggleMazeMode(bool mazeMode){  //Toggles if in "Main Menu" mode or "Maze" mode. Hides non-relevant UI elements while doing maze
        if (mazeMode){
            FastForwardButton.SetActive(true);
            MainMenu.SetActive(false);
            HelpMenu.SetActive(false);
            Headers.SetActive(false);
        } else {
            FastForwardButton.SetActive(false);
            MainMenu.SetActive(true);
            HelpMenu.SetActive(true);
            Headers.SetActive(true);
        }
    }

    public void ToggleFastForwardSpeed(){   //Speed up/Slow Down Maze Man's speed
        fastForwardActive = !fastForwardActive;

        if (fastForwardActive){
            FastForwardImage.sprite = PlayIcon;
            GameObject.Find("MazeMan").GetComponent<MazeManController>().UpdateSpeed(true);
        } else {
            FastForwardImage.sprite = FastForwardIcon;
            GameObject.Find("MazeMan").GetComponent<MazeManController>().UpdateSpeed(false);
        }
    }

    public void ToggleAudio(){  //Turn background music & sfx on/off
        audioOn = !audioOn;

        if (audioOn){
            audioImage.sprite = audioOnImg;
        } else {
            audioImage.sprite = audioOffImg;
        }

        //NEED TO ADD CALL TO GAMEMANAGER TO TOGGLE ON/OFF VOLUME!
    }

    public void Quit(){
        Application.Quit();
    }
}
