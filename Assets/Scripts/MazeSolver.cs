using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Takes in 2d char array maze, solves, optimizes, & returns Vector2 array for Maze Man to follow
public class MazeSolver : MonoBehaviour
{
    private List<Vector2> solvedPath = new List<Vector2>();

    public List<Vector2> SolveMaze(char[,] maze){
        solvedPath.Clear();

        if (RecursiveSearch(0, 1, maze, new bool[maze.GetLength(0), maze.GetLength(1)])){
            OptimizePath();
        }
            
        return solvedPath;
    }

    private bool RecursiveSearch(int x, int y, char[,] maze, bool[,] visited){
        bool finished = false;

        if (maze[x,y] == '='){    //Finished adding to list, exit recursion
            return true;
        }

        visited[x,y] = true;

        //Check each direction to see if empty & non-visited
        if (IsViableSpot(x, y+1, maze, visited))     //North
            finished = RecursiveSearch(x, y+1, maze, visited);
        if (!finished && IsViableSpot(x+1, y, maze, visited))  //East
            finished = RecursiveSearch(x+1, y, maze, visited);
        if (!finished && IsViableSpot(x, y-1, maze, visited))  //South
            finished = RecursiveSearch(x, y-1, maze, visited);
        if (!finished && (x != 0 || y != 1) && IsViableSpot(x-1, y, maze, visited))  //West, extra check to prevent breaking out of maze, .txt has open start
            finished = RecursiveSearch(x-1, y, maze, visited);

        if (finished){  //Add coordinates to List & return to previous recursion
            Vector2 position = new Vector2(x,y);
            solvedPath.Insert(0, position);
            return true;
        }
        return false;   //Dead End, loop back to last possible path
    }

    
    private bool IsViableSpot(int x, int y, char[,] maze, bool[,] visited){   //Check if space is empty or finish line, and that this spot has never been visited before
        return ((maze[x,y] == ' ' || maze[x,y] == '=') && !visited[x,y]);
    }

    private void OptimizePath(){    //Loop forwards & backwards through path to see if adjacent positions exist & then truncate
        /*int removeCount = 0;

        for (int a=0; a < (solvedPath.Count - removeCount) - 1; a++){
            for (int b=(solvedPath.Count - removeCount) - 1; b>removeCount; b--){
                if (a > (b+1) - removeCount || a < (b-1) - removeCount){
                    //errorCount++;
                    //Debug.Log(errorCount + " solvedA: " + solvedPath[a] + ", a: " + a);
                    //Debug.Log(errorCount + ", b next : " + b);
                    //Debug.Log(errorCount + " solvedB: " + solvedPath[b-removeCount] + ", b: " + b);


                    if ((solvedPath[a].x+1 == solvedPath[b-removeCount].x && solvedPath[a].y == solvedPath[b-removeCount].y) ||
                        (solvedPath[a].x-1 == solvedPath[b-removeCount].x && solvedPath[a].y == solvedPath[b-removeCount].y)){

                            Debug.Log(solvedPath[a].x+1);
                            if (b - removeCount > a){
                                for (int c = a+1; c < b - removeCount; c++){
                                    solvedPath.RemoveAt(c);
                                }
                            } else {
                                for (int c = (b - removeCount)+1; c < a; c++){
                                    solvedPath.RemoveAt(c);
                                }
                            }

                            removeCount++;
                    }    
                }
            }
        }*/         //Unfortunately out of time to solve problems with pathfinding optimization...
    }
}
