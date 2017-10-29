using UnityEngine;

namespace Tetris
{
    public interface Playfield
    {
        Rect WorldPlayArea { get; }

        void SetGridSize(int width, int height);
        Position PositionForWorldCoordinates(Vector3 worldCoordinates);
        Block BlockAtPosition(Position position);
        void AddBlockAtPosition(Position position, Color color);
        void RemoveBlockAtPosition(Position position);
        bool IsRowComplete(int row);
        bool IsRowEmpty(int row);
        void ClearRow(int row);
        int[] DeleteCompletedRows(int[] rowsToCheck);
        void ApplyGravity(int[] deletedRows);
    }
}

