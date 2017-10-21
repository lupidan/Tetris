using UnityEngine;

public struct TetrominoMovement
{
	public Tetromino Tetromino;
	public Vector3 PreviousPosition;
	public Vector3 PreviousRotation;
	public bool MovementInvolvesCommit;

	public bool DidMove
	{
		get
		{
			return this.Tetromino.transform.position != PreviousPosition || this.Tetromino.transform.eulerAngles != PreviousRotation;
		}
	}
}
