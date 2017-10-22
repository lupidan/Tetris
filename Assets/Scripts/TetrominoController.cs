using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public Tetromino ActiveTetromino;
    public GameArea GameArea;
    
    void Update () {
        if (ActiveTetromino == null || GameArea == null)
            return;

        TetrominoMovement movement = new TetrominoMovement()
        {
            Tetromino = ActiveTetromino,
            PreviousPosition = ActiveTetromino.transform.position,
            PreviousRotation = ActiveTetromino.transform.eulerAngles
        };

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ActiveTetromino.MoveRight();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ActiveTetromino.MoveLeft();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            ActiveTetromino.MoveUp();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActiveTetromino.HardDrop();
            movement.MovementInvolvesCommit = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActiveTetromino.MoveDown();
            movement.MovementInvolvesCommit = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
            ActiveTetromino.RotateClockwise();
            
        if (Input.GetKeyDown(KeyCode.X))
            ActiveTetromino.RotateCounterClockwise();

        if (movement.DidMove)
            AdjustTetrominoMovement(movement);
    }

    private void AdjustTetrominoMovement(TetrominoMovement movement)
    {
        LimitTetrominoMovementInPlayArea(movement);
    }

    private void LimitTetrominoMovementInPlayArea(TetrominoMovement movement)
    {
        Tetromino tetromino = movement.Tetromino;
        if (tetromino == null)
            return;
        
        Rect worldPlayArea = this.GameArea.WorldPlayArea;
        for(int i=0; i < tetromino.ChildBlocks.Length; ++i)
        {
            while (tetromino.ChildBlocks[i].transform.position.x < worldPlayArea.xMin)
                tetromino.MoveRight();

            while (tetromino.ChildBlocks[i].transform.position.x > worldPlayArea.xMax)
                tetromino.MoveLeft();

            while (tetromino.ChildBlocks[i].transform.position.y < worldPlayArea.yMin)
                tetromino.MoveUp();

            while (tetromino.ChildBlocks[i].transform.position.y > worldPlayArea.yMax)
                tetromino.MoveDown();
        }
    }
    }
}
