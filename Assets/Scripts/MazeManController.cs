using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Manage Mr. Maze Man's movement solving the maze
public class MazeManController : MonoBehaviour
{
    public float defaultSpeed;
    public float fastForwardSpeed;
    public Vector3 smallScale;

    private float speed;
    public Sprite greenDot;

    private bool running = false;
    private bool positionsPrimed = false;
    private List<Vector2> solvedPositions = new List<Vector2>();
    private Vector3 startingPos;

    private Animator animator;
    private AudioManager audioManager;

    void Start(){
        speed = defaultSpeed;
        startingPos = transform.position;  //Needed to reset between Maze Mode & Main Menu

        animator = GetComponent<Animator>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Update(){
        if (running){
            if (solvedPositions.Count > 0){    //If not yet at end, keep moving towards next position
                var step =  speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, solvedPositions[0], step);

                if (Vector3.Distance(transform.position, solvedPositions[0]) < 0.001f){
                    solvedPositions.RemoveAt(0);

                    if (solvedPositions.Count > 0 && transform.position.x < solvedPositions[0].x){
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);    //Face direction running
                    } else if (solvedPositions.Count > 0 && transform.position.x > solvedPositions[0].x) {
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }
            } else if (positionsPrimed) {   //Make sure finish animation only plays if the list properly populated
                animator.SetBool("MazeFinished", true);
            }
        }
    }

    public void BeginMaze(List<Vector2> positionList){
        animator.SetBool("MazeFinished", false);
        animator.SetTrigger("MazeMode");

        transform.localScale = smallScale;

        solvedPositions = positionList;
        solvedPositions.Add(new Vector2(solvedPositions[solvedPositions.Count-1].x+3, solvedPositions[solvedPositions.Count-1].y)); //Adds Victory spot beyond Finish Line

        positionsPrimed = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {   //Set dots for every blank space traveled & play sfx
        if (other.tag == "Blank"){
            other.GetComponent<SpriteRenderer>().sprite = greenDot;
            audioManager.PrepareFootstep();
        } else if (other.tag == "Finish"){
            audioManager.PrepareCheer();
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
        positionsPrimed = false;
        solvedPositions.Clear();

        transform.localScale = new Vector3(1,1,1);

        SetAnimationTrigger("ReturnToTitle");

        transform.position = startingPos;
    }

    public void SetAnimationTrigger(string trigger){
        animator.SetTrigger(trigger);
    }
}
