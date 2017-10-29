using System;
using UnityEngine;

namespace Tetris
{
    public interface Playfield
    {
        int Width { get; }
        int Height { get; }
        Rect WorldPlayArea { get; }

        void SetGridSize(int width, int height);
        void ClearGrid();
        Position PositionForWorldCoordinates(Vector3 worldCoordinates);
        Block BlockAtPosition(Position position);
        void AddBlockAtPosition(Position position, Color color, bool solid = true);
        void RemoveBlockAtPosition(Position position);
        bool IsRowComplete(int row);
        bool IsRowEmpty(int row);
        void ClearRow(int row);
        int[] DeleteCompletedRows(int[] rowsToCheck);
        void ApplyGravity(int[] deletedRows);
        void ApplyToBlocksInPlayfield(Action<Block> action);
    }
}

