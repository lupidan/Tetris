using UnityEngine;

namespace Tetris
{
    public class DefaultTetrominoSpawner : MonoBehaviour, TetrominoSpawner
    {
        [Header("Prefabs")]
        [SerializeField] private Tetromino[] _tetrominoPrefabs;

        #region TetrominoSpawner implementation
        public Tetromino SpawnRandomTetrominoAtPosition(Vector3 position)
        {
            Tetromino tetromino = Instantiate(TetrominoPrefabs[UnityEngine.Random.Range(0,7)]);
            tetromino.transform.position = position + tetromino.PositioningOffset;
            return tetromino;
        }

        public void DiscardTetromino(Tetromino tetromino)
        {
            Destroy(tetromino.gameObject);
        }
        #endregion
    }
}
