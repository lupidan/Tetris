using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayfieldImpl : MonoBehaviour, GamePlayfield
{
    public Rect LocalPlayArea { get { return _localPlayArea; } }
    public Rect WorldPlayArea
    {
        get
        {
            Vector2 offset = transform.position;
            return new Rect(_localPlayArea.position + offset, _localPlayArea.size);
        }
    }
    
    [Header("Components")]
    [SerializeField] private Transform _blocksParent;
    [SerializeField] private Transform _pooledBlocksParent;
    [Header("Prefabs")]
    [SerializeField] private Block _blockPrefab;

    private ComponentPool<Block> _blockPool;
    private Block[,] _blocks;
    private Rect _localPlayArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

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

        SetGridSize(10, 20);
    }

    #endregion

    #region GamePlayfield implementation
    public void SetGridSize(int width, int height)
    {
        if (width <= 4 || height <= 4)
            throw new Exception("Game area should be larger than 4x4.");

        ClearAllBlocks();

        _blocks = new Block[width + 2, height + 2];
        _localPlayArea = new Rect(1, 1, width, height);

        for (int x = 0; x < _blocks.GetLength(0); ++x)
        {
            AddBlockAtPosition(new GamePlayfieldPosition(x, 0), Color.white);
            AddBlockAtPosition(new GamePlayfieldPosition(x, _blocks.GetLength(1) - 1), Color.white);
        }

        for (int y = 1; y < _blocks.GetLength(1) - 1; ++y)
        {
            AddBlockAtPosition(new GamePlayfieldPosition(0, y), Color.white);
            AddBlockAtPosition(new GamePlayfieldPosition(_blocks.GetLength(0) - 1, y), Color.white);
        }
    }

    public GamePlayfieldPosition PositionForWorldCoordinates(Vector3 worldCoordinates)
    {
        Vector3 pieceLocalPosition = worldCoordinates - this.transform.position;
        return new GamePlayfieldPosition(Mathf.FloorToInt(pieceLocalPosition.x), Mathf.FloorToInt(pieceLocalPosition.y));
    }

    public Block BlockAtPosition(GamePlayfieldPosition position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= _blocks.GetLength(0) || position.y >= _blocks.GetLength(1))
            return null;

        return _blocks[position.x, position.y];
    }

    public void AddBlockAtPosition(GamePlayfieldPosition position, Color color)
    {
        if (position.x < 0 || position.y < 0 || position.x >= _blocks.GetLength(0) || position.y >= _blocks.GetLength(1))
            throw new Exception("Index (" + position.x + ", " + position.y + ") is out of bounds.");
        
        if (_blocks[position.x, position.y] != null)
            throw new Exception("A block already exists at (" + position.x + ", " + position.y + ").");

        Block block = _blockPool.Get();
        block.transform.localPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0.0f);
        block.Color = color;
        _blocks[position.x, position.y] = block;
    }

    public void RemoveBlockAtPosition(GamePlayfieldPosition position)
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
        int initialColumn = Mathf.RoundToInt(_localPlayArea.xMin);
        int endingColumn = Mathf.RoundToInt(_localPlayArea.xMax);
        for (int x = initialColumn; x < endingColumn; ++x)
            if (BlockAtPosition(new GamePlayfieldPosition(x, row)) == null)
                return false;
        return true;
    }

    public bool IsRowEmpty(int row)
    {
        int initialColumn = Mathf.RoundToInt(_localPlayArea.xMin);
        int endingColumn = Mathf.RoundToInt(_localPlayArea.xMax);
        for (int x = initialColumn; x < endingColumn; ++x)
            if (BlockAtPosition(new GamePlayfieldPosition(x, row)) != null)
                return false;
        return true;
    }

    public void ClearRow(int row)
    {
        int initialColumn = Mathf.RoundToInt(_localPlayArea.xMin);
        int endingColumn = Mathf.RoundToInt(_localPlayArea.xMax);
        for (int x = initialColumn; x < endingColumn; ++x)
            RemoveBlockAtPosition(new GamePlayfieldPosition(x, row));
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
        int rowMax = Mathf.RoundToInt(_localPlayArea.yMax);

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

    #endregion

    #region Private methods

    private void MoveRowDown(int y, int numberOfRowsDowns)
    {
        int initialColumn = Mathf.RoundToInt(_localPlayArea.xMin);
        int endingColumn = Mathf.RoundToInt(_localPlayArea.xMax);
        int destinationY = y - numberOfRowsDowns;
        for (int x = initialColumn; x < endingColumn; ++x)
        {
            Block block = _blocks[x, y];
            if (block != null)
            {
                _blocks[x, y] = null;
                _blocks[x, destinationY] = block;
                block.transform.DOLocalMoveY(destinationY + 0.5f, 0.5f).SetEase(Ease.OutBounce);
            }
        }
    }    

    private void ClearAllBlocks()
    {
        if (_blocks == null)
            return;

        for (int x = 0; x < _blocks.GetLength(0); ++x)
            for (int y = 0; y < _blocks.GetLength(1); ++y)
                if (_blocks[x, y] != null)
                    RemoveBlockAtPosition(new GamePlayfieldPosition(x, y));
    }

    #endregion
}
