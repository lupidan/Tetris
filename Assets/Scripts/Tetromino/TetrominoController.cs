using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class TetrominoController : MonoBehaviour
    {
        [Header("Config")]
        public float MoveDownInterval;

        [Header("Components")]
        public Tetromino ActiveTetromino;

        private float _autoMoveDownCounter = 0.0f;

        private GameController _gameController;
        public Playfield _playfield;
        private TetrominoSpawner _tetrominoSpawner;
        private Input _input;
        private ScoreController _scoreController;
        
        #region MonoBehaviour
        private void Start()
        {
            _scoreController.OnScoreUpdate += OnGameTimeUpdated;
        }

        private void Update ()
        {
            if (ActiveTetromino == null)
                return;
        
            _autoMoveDownCounter -= Time.deltaTime;

            if (_input.MoveLeft)
                TryMoveTetromino(ActiveTetromino, new Vector2(-1.0f, 0.0f));

            if (_input.MoveRight)
                TryMoveTetromino(ActiveTetromino, new Vector2(1.0f, 0.0f));
                
            if (_input.MoveDown || _autoMoveDownCounter <= 0.0f)
            {
                _autoMoveDownCounter = MoveDownInterval;
                bool couldMove = TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f));
                if (!couldMove)
                    PlaceTetrominoOnPlayfield(ActiveTetromino);
            }

            if (_input.HardDrop)
            {
                while(TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f)));
                PlaceTetrominoOnPlayfield(ActiveTetromino);
            }

            if (_input.RotateClockwise)
            {
                TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, -90.0f));
                ActiveTetromino.AdjustTetrominoChildBlocksRotation();
            }

            if (_input.RotateCounterClockwise)
            {
                TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, 90.0f));
                ActiveTetromino.AdjustTetrominoChildBlocksRotation();
            }
        }

        private void OnDestroy()
        {
            _gameController.OnGameTimeUpdate -= OnGameTimeUpdated;
        }

        #endregion

        #region Public methods

        public void Initialize(
            GameController gameController, 
            Playfield playfield,
            TetrominoSpawner tetrominoSpawner,
            Input input,
            ScoreController scoreController)
        {
            _gameController = gameController;
            _playfield = playfield;
            _tetrominoSpawner = tetrominoSpawner;
            _input = input;
            _scoreController = scoreController;
        }

        public void Run()
        {
            CreateRandomTetromino();
        }

        public void Stop()
        {
            if (ActiveTetromino != null)
                _tetrominoSpawner.DiscardTetromino(ActiveTetromino);
        }

        #endregion

        #region Private methods

        private void OnScoreUpdate(long score)
        {
            
        }

        private void CreateRandomTetromino()
        {
            Vector3 tetrominoPosition = Vector3.zero;
            tetrominoPosition.x = Mathf.Round(_playfield.WorldPlayArea.center.x);
            tetrominoPosition.y = Mathf.Round(_playfield.WorldPlayArea.yMax - 1.0f);
            tetrominoPosition.z = 1.0f;

            ActiveTetromino = _tetrominoSpawner.SpawnRandomTetrominoAtPosition(tetrominoPosition);
            _autoMoveDownCounter = MoveDownInterval;
        }

        private bool TryMoveTetromino(Tetromino tetromino, Vector2 moveVector)
        {
            Vector3 previousPosition = tetromino.transform.position;
            tetromino.transform.position = previousPosition + new Vector3(moveVector.x, moveVector.y, 0.0f);;

            for (int i = 0; i < tetromino.ChildBlocks.Length; ++i)
            {
                Vector3 blockWorldPosition = tetromino.ChildBlocks[i].transform.position;
                Position position = _playfield.PositionForWorldCoordinates(blockWorldPosition);
                Block blockAtPosition = _playfield.BlockAtPosition(position);
                if (blockAtPosition != null && blockAtPosition.IsSolid)
                {
                    tetromino.transform.position = previousPosition;
                    return false;
                }
            }
            return true;
        }

        private bool TryRotateTetromino(Tetromino tetromino, Vector3 eulerAngles)
        {
            Vector3 previousEulerAngles = tetromino.transform.rotation.eulerAngles;
            Vector3 newEulerAngles = previousEulerAngles + eulerAngles;
            tetromino.transform.rotation = Quaternion.Euler(newEulerAngles);

            Vector2[] testOffsets = SuperRotationSystem.GetTestOffsets(tetromino.WallKick, previousEulerAngles.z, newEulerAngles.z);
            for (int i = 0; i < testOffsets.Length; ++i)
            {
                if (TryMoveTetromino(tetromino, testOffsets[i]))
                    return true;
            }

            tetromino.transform.rotation = Quaternion.Euler(previousEulerAngles);
            return false;
        }

        private void PlaceTetrominoOnPlayfield(Tetromino tetromino)
        {
            try
            {
                HashSet<int> rowsToCheckSet = new HashSet<int>();
                for (int i = 0; i < tetromino.ChildBlocks.Length; ++i)
                {
                    Vector3 blockWorldPosition = tetromino.ChildBlocks[i].transform.position;
                    Position position = _playfield.PositionForWorldCoordinates(blockWorldPosition);
                    Color color = tetromino.ChildBlocks[i].Color;

                    _playfield.AddBlockAtPosition(position, color);
                    rowsToCheckSet.Add(position.y);
                }

                int[] rowsToCheck = rowsToCheckSet.ToArray();
                int[] deletedRows = _playfield.DeleteCompletedRows(rowsToCheck);
                _playfield.ApplyGravity(deletedRows);        
                _scoreController.UpdateScore(deletedRows, tetromino);

                _tetrominoSpawner.DiscardTetromino(tetromino);
                CreateRandomTetromino();
            }
            catch (Exception)
            {
                _playfield.ApplyToBlocksInPlayfield((block) =>
                {
                    if (block == null)
                        return;
                    
                    block.Color = Color.white;
                });
                _tetrominoSpawner.DiscardTetromino(tetromino);
                _gameController.GameOver();
                ActiveTetromino = null;
            }
        }

        #endregion
    }
}
