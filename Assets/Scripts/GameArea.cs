using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
	public Rect PlayArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	public void AdjustTetrominoPosition(Tetromino tetromino)
	{
		LimitTetrominoInPlayArea(tetromino);
	}

	private void LimitTetrominoInPlayArea(Tetromino tetromino)
	{
		if (tetromino == null)
			return;

		for(int i=0; i < tetromino.ChildBlocks.Length; ++i)
		{
			while (tetromino.ChildBlocks[i].position.x < PlayArea.xMin)
				tetromino.MoveRight();

			while (tetromino.ChildBlocks[i].position.x > PlayArea.xMax)
				tetromino.MoveLeft();

			while (tetromino.ChildBlocks[i].position.y < PlayArea.yMin)
				tetromino.MoveUp();

			while (tetromino.ChildBlocks[i].position.y > PlayArea.yMax)
				tetromino.MoveDown();
		}
	}
}
