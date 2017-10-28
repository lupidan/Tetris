using UnityEngine;

public interface GamePlayfield
{
	void SetGridSize(int width, int height);
	Block BlockAtPosition(GamePlayfieldPosition position);
	void AddBlockAtPosition(GamePlayfieldPosition position, Color color);
	void RemoveBlockAtPosition(GamePlayfieldPosition position);
	bool IsRowComplete(int row);
	bool IsRowEmpty(int row);
	void ClearRow(int row);
	int[] DeleteCompletedRows(int[] rowsToCheck);
	void ApplyGravity(int[] deletedRows);
}
