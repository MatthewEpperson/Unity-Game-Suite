using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoGameManager : MonoBehaviour
{
    private GoUI goUI;
    private GoBoard goBoard;
    private bool newMove = false;
    private bool update = false;
    
    // Start is called before the first frame update
    void Start()
    {
        goUI = GameObject.Find("GoBoard").GetComponent<GoUI>();
        goBoard = GameObject.Find("goBoard").GetComponent<GoBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) {
            // get mouse input move
            int currMoveX = 0;
            int currMoveY = 0;
            int turn = goBoard.getTurn();
            if (Input.GetMouseButtonDown(0)){
                Vector3 mousePos = Input.mousePosition;
                (currMoveX,currMoveY) = mouseInputMove(mousePos.x, mousePos.y);
                newMove = true;
            }

            bool resetCurrState = false;
            // check the validity of the new move if there is one
            if(newMove){
                // reset the newMove flag
                newMove = false;
                int[,] prevState = goBoard.getPrevState();
                goBoard.makeMove(currMoveX, currMoveY, turn);
                update = validMove(prevState, goBoard.getCurrState(), turn, (currMoveX, currMoveY));
                if(!update){
                    resetCurrState = true;
                }
            }

            // update the board
            if(update){
                update = false;
                // goBoard.makeMove(currMoveX,currMoveY, turn);
                int[,] currState = goBoard.getCurrState();
                goBoard.logCurrState(goBoard.deepCopy(currState));
                goBoard.nextTurn();
                goUI.printBoard(currState);
            }
            if(resetCurrState){
                resetCurrState = false;
                if(goBoard.getTurnCount() > 0){
                    goBoard.resetCurrState();
                }
            }
        }
    }

    public GoBoard getGoBoard(){
        return goBoard;
    }

    private (int, int) mouseInputMove(float mousePosX, float mousePosY){
        int x = 4;
        int y = 4;
        float gridSize = 108;
        float posX = mousePosX - 1920f/2;
        float posY = (1080-mousePosY) - 1080f/2;
        y += (int) Math.Round(posX/gridSize);
        x += (int) Math.Round(posY/gridSize);
        return (x,y);
    }

    private bool validMove(int[,] prevState, int[,] boardState, int turn, (int x, int y) currMove){
        if(prevState[currMove.x,currMove.y] != 0){
            return false;
        }
        //check liberty
        int liberty = 0;
        List<(int x,int y)> checkedPos = new List<(int x,int y)>();
        countLiberty(boardState, turn, currMove, checkedPos, ref liberty);
        bool alive = liberty>0 ? true:false;
        // check capture
        bool capture = captureMade(boardState, turn, currMove);
        // check ko
        bool ko = false;
        int turnCount = goBoard.getTurnCount();
        if(turnCount > 1){
            ko = checkKo(goBoard.getCurrState(), goBoard.getPrevPrevState());
        }
        return alive | (capture & !ko);
    }

    private void countLiberty(int[,] boardState, int turn, (int x,int y) currMove, List<(int x,int y)> checkedPos, ref int liberty){
        checkedPos.Add(currMove);
        List<(int x,int y)> adjacentPos = getAdjacentPos(currMove);
        foreach((int x,int y) pos in adjacentPos){
            if(inBoard(pos)){
                if(boardState[pos.x,pos.y] == turn & !checkedPos.Contains(pos)){
                    countLiberty(boardState, turn, pos, checkedPos, ref liberty);
                }
                else if(boardState[pos.x,pos.y] == 0){
                    liberty++;
                }
            }
        }
    }

    private List<(int x,int y)> getAdjacentPos((int x,int y) currMove){
        int currMoveX = currMove.x;
        int currMoveY = currMove.y;
        List<(int x,int y)> adjacentPos = new List<(int x,int y)>();
        adjacentPos.Add((currMoveX-1,currMoveY));
        adjacentPos.Add((currMoveX+1,currMoveY));
        adjacentPos.Add((currMoveX,currMoveY-1));
        adjacentPos.Add((currMoveX,currMoveY+1));
        return adjacentPos;
    }

    private bool inBoard((int x,int y) pos){
        if(pos.x >= 0 & pos.x <= 8 & pos.y >= 0 & pos.y <= 8){
            return true;
        }
        return false;
    }

    private bool captureMade(int[,] boardState, int turn, (int x, int y) currMove){
        bool captureMade = false;
        List<(int x,int y)> adjacentPos = getAdjacentPos(currMove);
        foreach((int x,int y) pos in adjacentPos){
            if(inBoard(pos)){
                if(boardState[pos.x,pos.y] == turn*-1){
                    List<(int x,int y)> checkedPos = new List<(int x,int y)>();
                    int liberty = 0;
                    countLiberty(boardState, turn*-1, pos, checkedPos, ref liberty);

                    if(liberty == 0){
                        captureMade = true;
                        foreach((int x, int y) position in checkedPos){
                            goBoard.clearMove(position.x, position.y);
                        }
                    }
                }
            }
        }
        return captureMade;
    }

    private bool checkKo(int[,] boardState, int[,] prevPrevState){
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                if(boardState[i,j] != prevPrevState[i,j]){
                    return false;
                }
            }
        }
        return true;
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