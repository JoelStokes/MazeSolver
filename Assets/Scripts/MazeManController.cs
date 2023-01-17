using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManController : MonoBehaviour
{
    public float speed = 1;
    public Sprite greenDot;

    private bool running = false;
    private List<Vector2> solvedPositions;

    void Update(){
        if (running){
            if (solvedPositions.Count > 0){    //If not yet at end, keep moving towards next position
                var step =  speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, solvedPositions[0], step);

                if (Vector3.Distance(transform.position, solvedPositions[0]) < 0.001f){
                    solvedPositions.RemoveAt(0);
                }
            } else {
                //FINISHED!
            }
        }
    }

    public void BeginMaze(List<Vector2> positionList){
        solvedPositions = positionList;   //Maybe a cleaner way to do this than a raw copy from a different script?
        running = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Blank"){
            other.GetComponent<SpriteRenderer>().sprite = greenDot;

            //Send Walking SFX to GameManager to see if Audio is turned on
        }
    }
}
