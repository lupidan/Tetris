using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
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

    public void SetGridSize(int width, int height)
    {
        if (width <= 4 || height <= 4)
            throw new Exception("Game area should be larger than 4x4.");

        ClearAllBlocks();

        _blocks = new Block[width + 2, height + 2];
        _localPlayArea = new Rect(1, 1, width, height);

        for (int x = 0; x < _blocks.GetLength(0); ++x)
        {
            AddBlockAtPosition(x, 0, Color.white);
            AddBlockAtPosition(x, _blocks.GetLength(1) - 1, Color.white);
        }

        for (int y = 1; y < _blocks.GetLength(1) - 1; ++y)
        {
            AddBlockAtPosition(0, y, Color.white);
            AddBlockAtPosition(_blocks.GetLength(0) - 1, y, Color.white);
        }
    }

    public void AddBlockAtPosition(int x, int y, Color color)
    {
        if (x < 0 || y < 0 || x >= _blocks.GetLength(0) || y >= _blocks.GetLength(1))
            throw new Exception("Index (" + x + ", " + y + ") is out of bounds.");
        
        if (_blocks[x, y] != null)
            throw new Exception("A block already exists at (" + x + ", " + y + ").");

        Block block = _blockPool.Get();
        block.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
        block.Color = color;
        _blocks[x, y] = block;
    }

    public void RemoveBlockAtPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _blocks.GetLength(0) || y >= _blocks.GetLength(1))
            throw new Exception("Index (" + x + ", " + y + ") is out of bounds.");
        
        if (_blocks[x,y] == null)
            throw new Exception("No block at (" + x + ", " + y + ").");

        Block block = _blocks[x, y];
        _blocks[x, y] = null;
        _blockPool.Return(block);
    }

    private void ClearAllBlocks()
    {
        if (_blocks == null)
            return;

        for (int x = 0; x < _blocks.GetLength(0); ++x)
            for (int y = 0; y < _blocks.GetLength(1); ++y)
                if (_blocks[x, y] != null)
                    RemoveBlockAtPosition(x, y);
    }
}
