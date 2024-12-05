using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Board board;
    public Flag myFlag;
    public bool myTurn;
    public void SetTeam(Flag flag)
    {
        myFlag = flag;
    }
    public void SetFlagOnBoard(int x,int y , Flag flag)
    {
        board.SetFlagOnBoard(x,y,flag);
    }
}
