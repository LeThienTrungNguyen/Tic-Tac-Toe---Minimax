using System;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Flag[,] board = new Flag[3,3];
    public bool gameover = false;
    public Flag Winner = Flag.None;
    public Transform boardPrefabs;
    public Transform XPrefabs;
    public Transform OPrefabs;
    public PlayerController player;
    public AIController AI;
    public TextMeshProUGUI playerWin;
    public TextMeshProUGUI AiWin;

    private void Awake()
    {
        CreateBoardLogic();
        DrawBoard();
        SetTeamToPlayers();
        AI.SetValidMoves();
    }
    void SetTeamToPlayers()
    {
        player.myFlag = Flag.X;
        player.myTurn = false;
        AI.myFlag = Flag.O;
        AI.myTurn = !player.myTurn;
    }
    public void SwitchTurn()
    {
        player.myTurn = !player.myTurn;
        AI.myTurn = !AI.myTurn;
        
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    

    private void ResetGame()
    {
        
        ClearBoard();
        CreateBoardLogic();
        DrawBoard();
        SetTeamToPlayers();
        AI.SetValidMoves();
        playerWin.enabled = false;
        AiWin.enabled = false;
    }

    private void ClearBoard()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        Winner = Flag.None;
        gameover = false;
    }

    void CreateBoardLogic()
    {
        for (int y = 0; y < 3; y++)
        {
            for(int x = 0; x < 3; x++)
            {
                board[x, y] = Flag.None;
            }
        }
    }
    void DrawBoard()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Instantiate(boardPrefabs, new Vector3(x, y, 0), Quaternion.identity, transform);
            }
        }
        { 
            Transform barrierHorizontal1 = Instantiate(boardPrefabs, new Vector3(1f, 0.5f, 0), Quaternion.identity, transform);
            barrierHorizontal1.localScale = new Vector3(2.8f, 0.05f, 1);
            barrierHorizontal1.GetComponent<SpriteRenderer>().color = Color.black;
            barrierHorizontal1.GetComponent<SpriteRenderer>().sortingOrder = 5;
            Transform barrierHorizontal2 = Instantiate(boardPrefabs, new Vector3(1f, 1.5f, 0), Quaternion.identity, transform);
            barrierHorizontal2.localScale = new Vector3(2.8f, 0.05f, 1);
            barrierHorizontal2.GetComponent<SpriteRenderer>().color = Color.black;
            barrierHorizontal2.GetComponent<SpriteRenderer>().sortingOrder = 5;

            Transform barrierVertical1 = Instantiate(boardPrefabs, new Vector3(0.5f, 1f, 0), Quaternion.identity, transform);
            barrierVertical1.localScale = new Vector3(0.05f, 2.8f, 1);
            barrierVertical1.GetComponent<SpriteRenderer>().color = Color.black;
            barrierVertical1.GetComponent<SpriteRenderer>().sortingOrder = 5;

            Transform barrierVertical2 = Instantiate(boardPrefabs, new Vector3(1.5f, 1f, 0), Quaternion.identity, transform);
            barrierVertical2.localScale = new Vector3(0.05f, 2.8f, 1);
            barrierVertical2.GetComponent<SpriteRenderer>().color = Color.black;
            barrierVertical2.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
    }
    public void SetFlagOnBoard(int x , int y , Flag type)
    {
        if (IsOutOfBound(x, y)) { Debug.Log("out of bound"); return; }
        if (board[x, y] != Flag.None) { Debug.Log("this position is placed"); return; }
        board[x, y] = type;
        AI.RemoveMoveFromValid(new Vector2Int(x, y));
        Instantiate(type == Flag.X ? XPrefabs : OPrefabs, new Vector3(x, y, 0), Quaternion.identity, transform);
        Winner = CheckBoardState(board);
        if (Winner != Flag.None) gameover = true; 
        else gameover = false;
        if (gameover) { 
            Debug.Log("Winner:"+ Winner); 
            if(Winner == player.myFlag) 
                playerWin.enabled = true;
            else if(Winner == AI.myFlag) 
                AiWin.enabled = true;
            return; 
        }
        SwitchTurn();
    }

    public Flag CheckBoardState(Flag[,] board)
    {
        var local_winner = Flag.None;
        // CHeck hang ngang
        for (int v = 0; v < 3; v++)
        {
            if (IsSameFlag(board[0, v], board[1, v]) && IsSameFlag(board[0, v], board[2, v])) { local_winner = board[0,v]; break; }
        }
        for(int h = 0; h < 3; h++)
        {
            if (IsSameFlag(board[h, 0], board[h, 1]) && IsSameFlag(board[h, 0], board[h, 2])) { local_winner = board[h,0]; break; }
        }
        int x = 1, y = 1;
        if (IsSameFlag(board[x, y], board[x - 1, y - 1]) && IsSameFlag(board[x, y], board[x + 1, y + 1])) { local_winner = board[x, y]; }
        if (IsSameFlag(board[x, y], board[x - 1, y + 1]) && IsSameFlag(board[x, y], board[x + 1, y - 1])) { local_winner = board[x, y]; }
        return local_winner;
    }
    public static bool IsOutOfBound(int x , int y)
    {
        return !(x >= 0 && x < 3 && y >= 0 && y < 3);
    }
    public bool IsSameFlag(Flag flag1 , Flag flag2)
    {
        return flag1 == flag2 && flag1 != Flag.None ;
    }
}
public enum Flag
{
    X,O,None
}
