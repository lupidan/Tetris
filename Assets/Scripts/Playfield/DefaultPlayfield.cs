using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class DefaultPlayfield : MonoBehaviour, Playfield
    {
        private const int BottomMargin = 2;
        private const int TopMargin = 2;
        private const int RightMargin = 2;
        private const int LeftMargin = 2;

        [Header("Components")]
        [SerializeField] private Transform _blocksParent;
        [SerializeField] private Transform _pooledBlocksParent;
        [Header("Prefabs")]
        [SerializeField] private Block _blockPrefab;

        private ComponentPool<Block> _blockPool;
        private Block[,] _blocks;

        #region MonoBehaviour methods

        public void Awake()
        {
            this._blockPool = new ComponentPool<Block>(25,
                () =>
                {
                    Block block = Instantiate(_blockPrefab);
                    block.gameObject.SetActive(false);
                    block.transform.SetParent(_pooledBlocksParent);
                    return block;
                },
                (block) => 
                {
                    block.gameObject.SetActive(true);
                    block.transform.SetParent(_blocksParent);
                },
                (block) =>
                {
                    block.gameObject.SetActive(false);
                    block.transform.SetParent(_pooledBlocksParent);
                });
        }

        #endregion

        #region GamePlayfield implementation
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rect LocalPlayArea { get; private set; }
        public Rect WorldPlayArea
        {
            get
            {
                Vector2 offset = transform.position;
                return new Rect(LocalPlayArea.position + offset, LocalPlayArea.size);
            }
        }

        public void SetGridSize(int width, int height)
        {
            if (width <= 4 || height <= 4)
                throw new Exception("Game area should be larger than 4x4.");

            _blocks = new Block[width + LeftMargin + RightMargin, height + BottomMargin + TopMargin];
            LocalPlayArea = new Rect(LeftMargin, BottomMargin, width, height);
            Width = width;
            Height = height;

            for (int y = 0; y < _blocks.GetLength(1); ++y)
            {
                for (int xLeft = 0; xLeft < LeftMargin; ++xLeft)
                    AddBlockAtPosition(new Position(xLeft, y), Color.white);

                for (int xRight = _blocks.GetLength(0) - RightMargin; xRight < _blocks.GetLength(0); ++xRight)
                    AddBlockAtPosition(new Position(xRight, y), Color.white);
            }

            for (int x = LeftMargin; x < _blocks.GetLength(0) - RightMargin; ++x)
            {
                for (int yBottom = 0; yBottom < BottomMargin; ++yBottom)
                    AddBlockAtPosition(new Position(x, yBottom), Color.white);

                for (int yTop = _blocks.GetLength(1) - TopMargin; yTop < _blocks.GetLength(1); ++yTop)
                    AddBlockAtPosition(new Position(x, yTop), Color.white, solid: false);
            }
        }

        public void ClearGrid()
        {
            if (_blocks == null)
                return;

            Width = 0;
            Height = 0;

            for (int x = 0; x < _blocks.GetLength(0); ++x)
                for (int y = 0; y < _blocks.GetLength(1); ++y)
                    if (_blocks[x, y] != null)
                        RemoveBlockAtPosition(new Position(x, y));
        }

        public Position PositionForWorldCoordinates(Vector3 worldCoordinates)
        {
            Vector3 pieceLocalPosition = worldCoordinates - this.transform.position;
            return new Position(Mathf.FloorToInt(pieceLocalPosition.x), Mathf.FloorToInt(pieceLocalPosition.y));
        }

        public Block BlockAtPosition(Position position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _blocks.GetLength(0) || position.y >= _blocks.GetLength(1))
                return null;

            return _blocks[position.x, position.y];
        }

        public void AddBlockAtPosition(Position position, Color color, bool solid = true)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _blocks.GetLength(0) || position.y >= _blocks.GetLength(1))
                throw new Exception("Index (" + position.x + ", " + position.y + ") is out of bounds.");
            
            if (_blocks[position.x, position.y] != null)
                throw new Exception("A block already exists at (" + position.x + ", " + position.y + ").");

            Block block = _blockPool.Get();
            block.transform.localPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0.0f);
            block.Color = color;
            block.IsSolid = solid;
            _blocks[position.x, position.y] = block;
        }

        public void RemoveBlockAtPosition(Position position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _blocks.GetLength(0) || position.y >= _blocks.GetLength(1))
                throw new Exception("Index (" + position.x + ", " + position.y + ") is out of bounds.");
            
            if (_blocks[position.x, position.y] == null)
                throw new Exception("No block at (" + position.x + ", " + position.y + ").");

            Block block = _blocks[position.x, position.y];
            _blocks[position.x, position.y] = null;
            _blockPool.Return(block);
        }

        public bool IsRowComplete(int row)
        {
            int initialColumn = Mathf.RoundToInt(LocalPlayArea.xMin);
            int endingColumn = Mathf.RoundToInt(LocalPlayArea.xMax);
            for (int x = initialColumn; x < endingColumn; ++x)
                if (BlockAtPosition(new Position(x, row)) == null)
                    return false;
            return true;
        }

        public bool IsRowEmpty(int row)
        {
            int initialColumn = Mathf.RoundToInt(LocalPlayArea.xMin);
            int endingColumn = Mathf.RoundToInt(LocalPlayArea.xMax);
            for (int x = initialColumn; x < endingColumn; ++x)
                if (BlockAtPosition(new Position(x, row)) != null)
                    return false;
            return true;
        }

        public void ClearRow(int row)
        {
            AnimateRowClear(row);

            int initialColumn = Mathf.RoundToInt(LocalPlayArea.xMin);
            int endingColumn = Mathf.RoundToInt(LocalPlayArea.xMax);
            for (int x = initialColumn; x < endingColumn; ++x)
                RemoveBlockAtPosition(new Position(x, row));
        }

        public int[] DeleteCompletedRows(int[] rowsToCheck)
        {
            if (rowsToCheck == null || rowsToCheck.Length == 0)
                return new int[0];

            List<int> deletedRows = new List<int>();
            for (int i = 0; i < rowsToCheck.Length; ++i)
            {
                int rowToCheck = rowsToCheck[i];
                if (IsRowComplete(rowToCheck))
                {
                    ClearRow(rowToCheck);
                    deletedRows.Add(rowToCheck);
                }
            }
            return deletedRows.ToArray();
        }

        public void ApplyGravity(int[] deletedRows)
        {
            if (deletedRows == null || deletedRows.Length == 0)
                return;

            Array.Sort(deletedRows);
            
            int numberOfEmptyLinesToIgnore = deletedRows.Length;
            int numberOfRowsToLower = 0;
            int rowMax = Mathf.RoundToInt(LocalPlayArea.yMax);

            for (int row = deletedRows[0]; row < rowMax; ++row)
            {
                if (IsRowEmpty(row))
                {
                    if (numberOfEmptyLinesToIgnore > 0)
                    {
                        numberOfRowsToLower += 1;
                        numberOfEmptyLinesToIgnore -= 1;
                    }
                    else
                    {
                        break;
                    }
                    
                }
                else if (numberOfRowsToLower > 0)
                {
                    MoveRowDown(row, numberOfRowsToLower);
                }
            }
        }

        public void ApplyToBlocksInPlayfield(Action<Block> action)
        {
            if (_blocks == null || action == null)
                return;

            for (int x = 0; x < _blocks.GetLength(0); ++x)
                for (int y = 0; y < _blocks.GetLength(1); ++y)
                    action(_blocks[x, y]);
        }

        #endregion

        #region Private methods

        private void MoveRowDown(int y, int numberOfRowsDowns, float animationTime = 0.5f)
        {
            Sequence animationSequence = DOTween.Sequence();

            int initialColumn = Mathf.RoundToInt(LocalPlayArea.xMin);
            int endingColumn = Mathf.RoundToInt(LocalPlayArea.xMax);
            int destinationY = y - numberOfRowsDowns;
            for (int x = initialColumn; x < endingColumn; ++x)
            {
                Block block = _blocks[x, y];
                if (block != null)
                {
                    _blocks[x, y] = null;
                    _blocks[x, destinationY] = block;
                    animationSequence.Insert(
                        0.0f, 
                        block.transform.DOLocalMoveY(destinationY + 0.5f, animationTime).SetEase(Ease.OutBounce));
                    
                }
            }

            animationSequence.Play();
        }

        private void AnimateRowClear(int y, float animationTime = 0.5f)
        {
            Sequence sequence = DOTween.Sequence();

            int initialColumn = Mathf.RoundToInt(LocalPlayArea.xMin);
            int endingColumn = Mathf.RoundToInt(LocalPlayArea.xMax);
            for (int x = initialColumn; x < endingColumn; ++x)
            {
                Block clearedBlock = _blocks[x, y];

                Block animatedBlock = _blockPool.Get();
                Vector3 initialPosition = clearedBlock.transform.position;
                initialPosition.z = -2.0f;
                Color initialColor = clearedBlock.Color;
                Color endingColor = clearedBlock.Color;
                endingColor.a = 0.0f;

                animatedBlock.transform.position = initialPosition;
                animatedBlock.Color = initialColor;

                Tweener positionTweener = animatedBlock.transform.DOMoveY(y - 0.5f, animationTime)
                    .OnComplete(() => _blockPool.Return(animatedBlock));

                Tweener colorTweener = DOTween.To(
                    () => animatedBlock.Color,
                    (interpolatedColor) => animatedBlock.Color = interpolatedColor,
                    endingColor,
                    animationTime);

                sequence.Insert(0.0f, positionTweener);
                sequence.Insert(0.0f, colorTweener);
            }

            sequence.Play();                
        }

        #endregion
    }
}
