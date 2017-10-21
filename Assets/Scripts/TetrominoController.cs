using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
	public Tetromino ActiveTetromino;
	public GameArea GameArea;
	
	// Update is called once per frame
	void Update () {
		if (this.ActiveTetromino == null || this.GameArea == null)
			return;
			
		if (Input.GetKeyDown(KeyCode.RightArrow))
			this.ActiveTetromino.MoveRight();
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			this.ActiveTetromino.MoveLeft();
		if (Input.GetKeyDown(KeyCode.UpArrow))
			this.ActiveTetromino.HardDrop();
		if (Input.GetKeyDown(KeyCode.DownArrow))
			this.ActiveTetromino.MoveDown();
		if (Input.GetKeyDown(KeyCode.Z))
			this.ActiveTetromino.RotateClockwise();
		if (Input.GetKeyDown(KeyCode.X))
			this.ActiveTetromino.RotateCounterClockwise();

		this.GameArea.AdjustTetrominoPosition(this.ActiveTetromino);
	}

}
