using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
	public Rect PlayArea { get { return _playArea; } }
	
	[SerializeField] private Rect _playArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

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
			while (tetromino.ChildBlocks[i].position.x < this._playArea.xMin)
				tetromino.MoveRight();

			while (tetromino.ChildBlocks[i].position.x > this._playArea.xMax)
				tetromino.MoveLeft();

			while (tetromino.ChildBlocks[i].position.y < this._playArea.yMin)
				tetromino.MoveUp();

			while (tetromino.ChildBlocks[i].position.y > this._playArea.yMax)
				tetromino.MoveDown();
		}
	}
}
