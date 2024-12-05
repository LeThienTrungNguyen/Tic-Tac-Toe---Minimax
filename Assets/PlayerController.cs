using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public Vector2Int mouseBoardPosition;
    
    private void Update()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseBoardPosition = ToVector2Int(mouseWorldPosition);
        if (Board.IsOutOfBound(mouseBoardPosition.x, mouseBoardPosition.y)) { mouseBoardPosition = new Vector2Int(-99999, -99999); }
        if (Input.GetMouseButtonDown(0) && myTurn && !board.gameover)
        {
            if (!Board.IsOutOfBound(mouseBoardPosition.x, mouseBoardPosition.y)) board.SetFlagOnBoard(mouseBoardPosition.x, mouseBoardPosition.y, myFlag);
        }
    }
    Vector2Int ToVector2Int(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        return new Vector2Int(x,y);
    }
}
