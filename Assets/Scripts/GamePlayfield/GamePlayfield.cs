using UnityEngine;

public interface GamePlayfield
{
	Rect WorldPlayArea { get; }

	void SetGridSize(int width, int height);
	GamePlayfieldPosition PositionForWorldCoordinates(Vector3 worldCoordinates);
	Block BlockAtPosition(GamePlayfieldPosition position);
	void AddBlockAtPosition(GamePlayfieldPosition position, Color color);
	void RemoveBlockAtPosition(GamePlayfieldPosition position);
	bool IsRowComplete(int row);
	bool IsRowEmpty(int row);
	void ClearRow(int row);
	int[] DeleteCompletedRows(int[] rowsToCheck);
	void ApplyGravity(int[] deletedRows);
}
