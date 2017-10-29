﻿using System.Collections;
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
        

        [Header("Prefabs")]
        public Tetromino[] TetrominoPrefabs;

        private float _autoMoveDownCounter = 0.0f;

        private Input _input;
        public Playfield _playfield;
        private ScoreController _scoreController;
        
        #region MonoBehaviour

        void Update ()
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

        #endregion

        #region Public methods

        public void Initialize(Playfield playfield, Input input, ScoreController scoreController)
        {
            _playfield = playfield;
            _input = input;
            _scoreController = scoreController;
        }

        public void CreateRandomTetromino()
        {
            Tetromino instantiatedTetromino = Instantiate(TetrominoPrefabs[UnityEngine.Random.Range(0,7)]);
            Vector3 tetrominoPosition = Vector3.zero;
            tetrominoPosition.x = Mathf.Round(_playfield.WorldPlayArea.center.x) + instantiatedTetromino.PositioningOffset.x;
            tetrominoPosition.y = Mathf.Round(_playfield.WorldPlayArea.yMax - 1.0f) + instantiatedTetromino.PositioningOffset.y;
            tetrominoPosition.z = 1.0f;
            instantiatedTetromino.transform.position = tetrominoPosition;

            ActiveTetromino = instantiatedTetromino;
            _autoMoveDownCounter = MoveDownInterval;
        }

        #endregion

        #region Private methods

        private bool TryMoveTetromino(Tetromino tetromino, Vector2 moveVector)
        {
            Vector3 previousPosition = ActiveTetromino.transform.position;
            ActiveTetromino.transform.position = previousPosition + new Vector3(moveVector.x, moveVector.y, 0.0f);;

            for (int i = 0; i < tetromino.ChildBlocks.Length; ++i)
            {
                Vector3 blockWorldPosition = tetromino.ChildBlocks[i].transform.position;
                Position position = _playfield.PositionForWorldCoordinates(blockWorldPosition);
                Block blockAtPosition = _playfield.BlockAtPosition(position);
                if (blockAtPosition != null && blockAtPosition.IsSolid)
                {
                    ActiveTetromino.transform.position = previousPosition;
                    return false;
                }
            }
            return true;
        }

        private bool TryRotateTetromino(Tetromino tetromino, Vector3 eulerAngles)
        {
            Vector3 previousEulerAngles = ActiveTetromino.transform.rotation.eulerAngles;
            Vector3 newEulerAngles = previousEulerAngles + eulerAngles;
            ActiveTetromino.transform.rotation = Quaternion.Euler(newEulerAngles);

            Vector2[] testOffsets = SuperRotationSystem.GetTestOffsets(tetromino.WallKick, previousEulerAngles.z, newEulerAngles.z);
            for (int i = 0; i < testOffsets.Length; ++i)
            {
                if (TryMoveTetromino(tetromino, testOffsets[i]))
                    return true;
            }

            ActiveTetromino.transform.rotation = Quaternion.Euler(previousEulerAngles);
            return false;
        }

        private void PlaceTetrominoOnPlayfield(Tetromino tetromino)
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

            Destroy(tetromino.gameObject);
            CreateRandomTetromino();
        }

        #endregion
    }
}