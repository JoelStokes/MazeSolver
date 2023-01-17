using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManController : MonoBehaviour
{
    public float defaultSpeed;
    public float fastForwardSpeed;

    private float speed;
    public Sprite greenDot;

    private bool running = false;
    private List<Vector2> solvedPositions;
    private Vector3 startingPos;

    void Start(){
        speed = defaultSpeed;
        startingPos = transform.position;  //Needed to reset between Maze Mode & Main Menu
    }

    void Update(){
        if (running){
            if (solvedPositions.Count > 0){    //If not yet at end, keep moving towards next position
                var step =  speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, solvedPositions[0], step);

                if (Vector3.Distance(transform.position, solvedPositions[0]) < 0.001f){
                    solvedPositions.RemoveAt(0);
                }
            } else {
                //FINISHED
            }
        }
    }

    public void BeginMaze(List<Vector2> positionList){
        solvedPositions = positionList;   //Maybe a cleaner way to do this than a raw copy from a different script?
        solvedPositions.Add(new Vector2(solvedPositions[solvedPositions.Count-1].x+3, solvedPositions[solvedPositions.Count-1].y)); //Add Victory spot beyond Finish Line
        running = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Blank"){
            other.GetComponent<SpriteRenderer>().sprite = greenDot;

            //Send Walking SFX to GameManager to see if Audio is turned on
        } else if (other.tag == "Finish"){
            
            //Play Victory Jingle
        }
    }

    public void UpdateSpeed(bool fastForward){
        if (fastForward){
            speed = fastForwardSpeed;
        } else {
            speed = defaultSpeed;
        }
    }

    public void Restart(){  //Clear out maze info & return to Main Menu position
        running = false;
        solvedPositions.Clear();

        transform.position = startingPos;
    }
}
