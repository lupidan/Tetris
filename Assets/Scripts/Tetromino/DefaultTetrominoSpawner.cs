using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class DefaultTetrominoSpawner : MonoBehaviour, TetrominoSpawner
    {
        [Header("Prefabs")]
        [SerializeField] private Tetromino[] _tetrominoPrefabs;

        private ComponentPool<Tetromino>[] _pools;
        private List<int> _prefabIndexesToSpawn = new List<int>();

        #region TetrominoSpawner implementation
        private void Awake()
        {
            List<ComponentPool<Tetromino>> createdPools = new List<ComponentPool<Tetromino>>();

            for (int i = 0; i < _tetrominoPrefabs.Length; ++i)
            {
                Tetromino prefabToPool = _tetrominoPrefabs[i];
                ComponentPool<Tetromino> dedicatedPool = new ComponentPool<Tetromino>(
                    1,
                    () =>
                    {
                        Tetromino tetromino = Instantiate(prefabToPool);
                        tetromino.gameObject.SetActive(false);
                        return tetromino;
                    },
                    (tetromino) => 
                    {
                        tetromino.gameObject.SetActive(true);
                        tetromino.transform.rotation = Quaternion.identity;
                        tetromino.AdjustTetrominoChildBlocksRotation();
                    },
                    (tetromino) =>
                    {
                        tetromino.gameObject.SetActive(false);
                    });
                createdPools.Add(dedicatedPool);
            }

            _pools = createdPools.ToArray();
        }
        #endregion

        #region TetrominoSpawner implementation
        public Tetromino SpawnRandomTetrominoAtPosition(Vector3 position)
        {
            if (_prefabIndexesToSpawn.Count < 2)
                AddSetOfRandomPrefabsToSpawn();
            
            int prefabIndex = _prefabIndexesToSpawn[0];
            _prefabIndexesToSpawn.RemoveAt(0);

            Tetromino tetromino = _pools[prefabIndex].Get();
            tetromino.transform.position = position + tetromino.PositioningOffset;
            return tetromino;
        }

        public void DiscardTetromino(Tetromino tetromino)
        {
            Destroy(tetromino.gameObject);
        }
        #endregion

        #region Private methods
        private void AddSetOfRandomPrefabsToSpawn()
        {
            List<int> indexesToAdd = new List<int>();
            for (int i = 0; i < _tetrominoPrefabs.Length; ++i)
                indexesToAdd.Add(i);
            
            indexesToAdd.Shuffle();
            _prefabIndexesToSpawn.AddRange(indexesToAdd);
        }
        #endregion
    }
}
