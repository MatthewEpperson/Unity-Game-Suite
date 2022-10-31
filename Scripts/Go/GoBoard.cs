using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBoard : MonoBehaviour
{
    private int[,] currState = new int[9,9];
    private List<int[,]> prevStates = new List<int[,]>();
    private int turn = 1;
    private int turnCount = 0;

    void Start(){
        prevStates.Add(deepCopy(currState));
    }

    public void makeMove(int x, int y, int turn){
        currState[x,y] = turn;
    }

    public void clearMove(int x, int y){
        currState[x,y] = 0;
    }

    public int getTurn(){
        return turn;
    }
    public void nextTurn(){
        turn *= -1;
        turnCount++;
    }

    public void resetCurrState(){
        currState = deepCopy(prevStates[prevStates.Count-1]);
    }
    public int[,] getCurrState(){
        return currState;
    }

    public int[,] getPrevState(){
        return prevStates[prevStates.Count-1];
    }

    public int[,] getPrevPrevState(){
        return prevStates[prevStates.Count-2];
    }

    public void logCurrState(int[,] currState){
        prevStates.Add(currState);
    }

    public int getTurnCount(){
        return turnCount;
    }

    public int[,] deepCopy(int[,] boardState){
        int[,] deepCopy = new int[9,9];
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                deepCopy[i,j] = boardState[i,j];
            }
        }
        return deepCopy;
    }
    public static void Print2DArray<T>(T[,] matrix)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for(int i=0; i<9; i++)
        {
            for(int j=0; j<9; j++)
            {
                sb.Append(matrix[i,j]);
                sb.Append(' ');				   
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}