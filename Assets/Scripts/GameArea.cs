using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    public Rect PlayArea { get { return _playArea; } }
    
    [SerializeField] private Rect _playArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    [SerializeField] private Transform _blocksParent;
    [SerializeField] private Transform _pooledBlocksParent;
    [SerializeField] private Block _blockPrefab;

    private ComponentPool<Block> _blockPool;

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

    public void AdjustTetrominoMovement(TetrominoMovement movement)
    {
        LimitTetrominoMovementInPlayArea(movement);
    }

    private void LimitTetrominoMovementInPlayArea(TetrominoMovement movement)
    {
        Tetromino tetromino = movement.Tetromino;
        if (tetromino == null)
            return;
        
        for(int i=0; i < tetromino.ChildBlocks.Length; ++i)
        {
            while (tetromino.ChildBlocks[i].position.x < this._playArea.xMin)
                tetromino.MoveRight();

            while (tetromino.ChildBlocks[i].position.x > this._playArea.xMax)
                tetromino.MoveLeft();

            while (tetromino.ChildBlocks[i].position.y < this._playArea.yMin)
                tetromino.MoveUp();

            while (tetromino.ChildBlocks[i].position.y > this._playArea.yMax)
                tetromino.MoveDown();
        }
    }
}
