using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    [Header("Config")]
    public float MoveDownInterval;

    [Header("Components")]
    public Tetromino ActiveTetromino;
    public GameArea GameArea;

    [Header("Prefabs")]
    public Tetromino[] TetrominoPrefabs;

    private float _autoMoveDownCounter = 0.0f;

    private GameInput _gameInput;
    private ScoreController _scoreController;
    
    #region MonoBehaviour

    void Start()
    {
        CreateRandomTetromino();
    }

    void Update () {
        if (ActiveTetromino == null || GameArea == null)
            return;
    
        _autoMoveDownCounter -= Time.deltaTime;

        if (_gameInput.MoveLeft)
            TryMoveTetromino(ActiveTetromino, new Vector2(-1.0f, 0.0f));

        if (_gameInput.MoveRight)
            TryMoveTetromino(ActiveTetromino, new Vector2(1.0f, 0.0f));
            
        if (_gameInput.MoveDown || _autoMoveDownCounter <= 0.0f)
        {
            _autoMoveDownCounter = MoveDownInterval;
            bool couldMove = TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f));
            if (!couldMove)
                PlaceTetrominoOnPlayArea(ActiveTetromino);
        }

        if (_gameInput.HardDrop)
        {
            while(TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f)));
            PlaceTetrominoOnPlayArea(ActiveTetromino);
        }

        if (_gameInput.RotateClockwise)
        {
            TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, -90.0f));
            AdjustTetriminoChildBlocksRotation(ActiveTetromino);
        }

        if (_gameInput.RotateCounterClockwise)
        {
            TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, 90.0f));
            AdjustTetriminoChildBlocksRotation(ActiveTetromino);
        }
    }

    #endregion

    #region Public methods
    public void Initialize(GameInput gameInput, ScoreController scoreController)
    {
        _gameInput = gameInput;
        _scoreController = scoreController;
    }
    #endregion

    #region Private methods

    private bool TryMoveTetromino(Tetromino tetromino, Vector2 moveVector)
    {
        Vector3 previousPosition = ActiveTetromino.transform.position;
        ActiveTetromino.transform.position = previousPosition + new Vector3(moveVector.x, moveVector.y, 0.0f);;

        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
        {
            Vector3 pieceLocalPosition = tetromino.ChildBlocks[i].transform.position - GameArea.transform.position;
            int x = Mathf.FloorToInt(pieceLocalPosition.x);
            int y = Mathf.FloorToInt(pieceLocalPosition.y);
            if (GameArea.BlockAtPosition(x, y) != null)
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

        Vector2[] testOffsets = SuperRotationSystem.GetTestOffsets(tetromino.WallKickTypee, previousEulerAngles.z, newEulerAngles.z);
        for (int i = 0; i < testOffsets.Length; i++)
        {
            if (TryMoveTetromino(tetromino, testOffsets[i]))
                return true;
        }

        ActiveTetromino.transform.rotation = Quaternion.Euler(previousEulerAngles);
        return false;
    }

    private void AdjustTetriminoChildBlocksRotation(Tetromino tetromino)
    {
        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
            tetromino.ChildBlocks[i].transform.rotation = Quaternion.identity;
    }

    private void PlaceTetrominoOnPlayArea(Tetromino tetromino)
    {
        HashSet<int> rowsToCheck = new HashSet<int>();
        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
        {
            Vector3 pieceLocalPosition = tetromino.ChildBlocks[i].transform.position - GameArea.transform.position;

            int x = Mathf.FloorToInt(pieceLocalPosition.x);
            int y = Mathf.FloorToInt(pieceLocalPosition.y);
            Color color = tetromino.ChildBlocks[i].Color;
            GameArea.AddBlockAtPosition(x, y, color);
            rowsToCheck.Add(y);
        }

        DeleteCompletedRows(rowsToCheck);
        _scoreController.UpdateScore(rowsToCheck.ToArray(), tetromino);
        GameArea.ApplyGravity();

        Destroy(tetromino.gameObject);
        CreateRandomTetromino();
    }

    private void DeleteCompletedRows(HashSet<int> rowsToCheck)
    {
        foreach(int y in rowsToCheck)
        {
            if (GameArea.IsRowComplete(y))
            {
                GameArea.ClearRow(y);
            }
        }
    }

    private void CreateRandomTetromino()
    {
        Tetromino instantiatedTetromino = Instantiate(TetrominoPrefabs[UnityEngine.Random.Range(0,7)]);
        Vector3 tetrominoPosition = Vector3.zero;
        tetrominoPosition.x = GameArea.WorldPlayArea.center.x + instantiatedTetromino.PositioningOffset.x;
        tetrominoPosition.y = GameArea.WorldPlayArea.yMax - 4.0f + instantiatedTetromino.PositioningOffset.y;
        instantiatedTetromino.transform.position = tetrominoPosition;

        ActiveTetromino = instantiatedTetromino;
        _autoMoveDownCounter = MoveDownInterval;
    }

    #endregion
}
