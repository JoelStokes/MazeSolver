using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManController : MonoBehaviour
{
    public float defaultSpeed;
    public float fastForwardSpeed;
    public Vector3 smallScale;

    private float speed;
    public Sprite greenDot;

    private bool running = false;
    private List<Vector2> solvedPositions;
    private Vector3 startingPos;

    private Animator animator;

    void Start(){
        speed = defaultSpeed;
        startingPos = transform.position;  //Needed to reset between Maze Mode & Main Menu

        animator = GetComponent<Animator>();
    }

    void Update(){
        if (running){
            if (solvedPositions.Count > 0){    //If not yet at end, keep moving towards next position
                var step =  speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, solvedPositions[0], step);

                if (Vector3.Distance(transform.position, solvedPositions[0]) < 0.001f){
                    solvedPositions.RemoveAt(0);

                    if (solvedPositions.Count > 0 && transform.position.x - solvedPositions[0].x < -0.05f){
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);    //Face direction running
                    } else if (solvedPositions.Count > 0 && transform.position.x - solvedPositions[0].x > 0.05f) {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }
            } else {
                SetAnimationTrigger("MazeFinished");
            }
        }
    }

    public void BeginMaze(List<Vector2> positionList){
        animator.SetTrigger("MazeMode");

        transform.localScale = smallScale;

        solvedPositions = positionList;
        solvedPositions.Add(new Vector2(solvedPositions[solvedPositions.Count-1].x+3, solvedPositions[solvedPositions.Count-1].y)); //Adds Victory spot beyond Finish Line
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

    public void BeginRun(){     //Called by Animator after Maze Man finishes Stretch
        running = true;
    }

    public void Restart(){  //Clear out maze info & return to Main Menu position
        running = false;
        solvedPositions.Clear();

        transform.localScale = new Vector3(1,1,1);

        SetAnimationTrigger("ReturnToTitle");

        transform.position = startingPos;
    }

    public void SetAnimationTrigger(string trigger){
        animator.SetTrigger(trigger);
    }
}
