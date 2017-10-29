using UnityEngine;

namespace Tetris
{
    public interface TetrominoSpawner
    {
        Tetromino SpawnRandomTetrominoAtPosition(Vector3 position);
        void DiscardTetromino(Tetromino tetromino);
    }
}
