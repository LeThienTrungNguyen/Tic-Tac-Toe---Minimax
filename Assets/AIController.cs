using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public List<Vector2Int> validMoves = new List<Vector2Int>();
    public int predictStepMax;
    public void SetValidMoves()
    {
        validMoves = validMovePosition(Board.board);
    }
    private void FixedUpdate()
    {
        if (board.gameover) return;
        if (!myTurn) return;
        TryMove();
    }
    void TryMove()
    {
        if (validMoves.Count == 0) return;
        var bestMove = GetBestMove(Board.board, validMoves, 10);
        board.SetFlagOnBoard(bestMove.x, bestMove.y, myFlag);
    }
    public Vector2Int GetBestMove(Flag[,] board,List<Vector2Int> validMoves,int predictStepMax)
    {
        var bestMove = new Vector2Int(-1, -1);
        int bestScore = int.MinValue;
        foreach (Vector2Int move in validMoves)
        {
            board[move.x, move.y] = myFlag; 
            int score = Minimax(board, predictStepMax - 1, false);
            board[move.x, move.y] = Flag.None; 
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }
        return bestMove;
    }

    public void RemoveMoveFromValid(Vector2Int index)
    {
        if (validMoves.Count > 0)
        {
            //Debug.Log($"I cant fill my flag in this position [{index.x},{index.y}]");
            validMoves.Remove(index);
        }
    }
    public List<Vector2Int> validMovePosition(Flag[,] board)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        for(int y = 0; y< 3; y++)
        {
            for(int x = 0;x< 3; x++)
            {
                if (board[x, y] == Flag.None)
                {
                    validMoves.Add(new Vector2Int(x,y));
                    //Debug.Log($"Add [{x},{y}] to valid moves");
                }
            }
        }
        return validMoves;
    }

    private int Minimax(Flag[,] board, int depth , bool isMaximizing)
    {
        Flag winner = this.board.CheckBoardState(board);
        if (winner == myFlag) return 10; 
        if (winner == (myFlag == Flag.X ? Flag.O : Flag.X)) return -10; 
        if (depth == 0 || IsBoardFull(board))  return 0; 
        
        if(isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach(Vector2Int move in validMovePosition(board))
            {
                board[move.x, move.y] = myFlag;
                int eval = Minimax(board, depth - 1, false);
                board[move.x, move.y] = Flag.None;
                maxEval = Mathf.Max(maxEval, eval);
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            Flag opponentFlag = (myFlag == Flag.X) ? Flag.O : Flag.X;
            foreach(Vector2Int move in validMovePosition(board))
            {
                board[move.x, move.y] = opponentFlag;
                int eval = Minimax(board, depth - 1, true);
                board[move.x, move.y] = Flag.None;
                minEval = Mathf.Min(minEval, eval);
            }
            return minEval;
        }
    }

    private bool IsBoardFull(Flag[,] board)
    {
        foreach(Flag flag in board)
        {
            if (flag == Flag.None) return false;
        }
        return true;
    }
}
