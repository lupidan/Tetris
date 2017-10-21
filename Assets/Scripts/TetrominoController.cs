using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public Tetromino ActiveTetromino;
    public GameArea GameArea;
    
    void Update () {
        if (this.ActiveTetromino == null || this.GameArea == null)
            return;

        TetrominoMovement movement = new TetrominoMovement()
        {
            Tetromino = this.ActiveTetromino,
            PreviousPosition = this.ActiveTetromino.transform.position,
            PreviousRotation = this.ActiveTetromino.transform.eulerAngles
        };

        if (Input.GetKeyDown(KeyCode.RightArrow))
            this.ActiveTetromino.MoveRight();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            this.ActiveTetromino.MoveLeft();

        if (Input.GetKeyDown(KeyCode.Q))
            this.ActiveTetromino.MoveUp();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.ActiveTetromino.HardDrop();
            movement.MovementInvolvesCommit = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.ActiveTetromino.MoveDown();
            movement.MovementInvolvesCommit = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
            this.ActiveTetromino.RotateClockwise();
            
        if (Input.GetKeyDown(KeyCode.X))
            this.ActiveTetromino.RotateCounterClockwise();

        if (movement.DidMove)
            this.GameArea.AdjustTetrominoMovement(movement);
    }
}
