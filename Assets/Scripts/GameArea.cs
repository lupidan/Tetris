using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    public Rect PlayArea { get { return _playArea; } }
    
    [SerializeField] private Rect _playArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    [SerializeField] private Transform _piecesParent;
    [SerializeField] private Transform _pooledPiecesParent;
    [SerializeField] private Piece _piecePrefab;

    private ComponentPool<Piece> _piecePool;

    public void Awake()
    {
        this._piecePool = new ComponentPool<Piece>(25,
            () =>
            {
                Piece piece = Instantiate(_piecePrefab);
                piece.gameObject.SetActive(false);
                piece.transform.SetParent(_pooledPiecesParent);
                return piece;
            },
            (piece) => 
            {
                piece.gameObject.SetActive(true);
                piece.transform.SetParent(_piecesParent);
            },
            (piece) =>
            {
                piece.gameObject.SetActive(false);
                piece.transform.SetParent(_pooledPiecesParent);
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
