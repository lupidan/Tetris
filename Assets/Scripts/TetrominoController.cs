using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public Tetromino ActiveTetromino;
    public GameArea GameArea;
    
    void Update () {
        if (ActiveTetromino == null || GameArea == null)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMoveTetromino(ActiveTetromino, new Vector2(1.0f, 0.0f));

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMoveTetromino(ActiveTetromino, new Vector2(-1.0f, 0.0f));

        if (Input.GetKeyDown(KeyCode.UpArrow))
            TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, 1.0f));

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool couldMove = TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f));
            if (!couldMove)
            {
                PlaceTetrominoOnPlayArea(ActiveTetromino);
                ActiveTetromino.transform.position = new Vector3(6.0f, 18.0f, 0.0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            while(TryMoveTetromino(ActiveTetromino, new Vector2(0.0f, -1.0f)));
            PlaceTetrominoOnPlayArea(ActiveTetromino);
            ActiveTetromino.transform.position = new Vector3(6.0f, 18.0f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
            TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, -90.0f));
            
        if (Input.GetKeyDown(KeyCode.X))
            TryRotateTetromino(ActiveTetromino, new Vector3(0.0f, 0.0f, 90.0f));
    }

    private void LimitTetrominoInsidePlayArea(Tetromino tetromino)
    {
        if (tetromino == null)
            return;
        
        Rect worldPlayArea = this.GameArea.WorldPlayArea;
        for(int i=0; i < tetromino.ChildBlocks.Length; ++i)
        {
            while (tetromino.ChildBlocks[i].transform.position.x < worldPlayArea.xMin)
                tetromino.transform.position += new Vector3(1.0f, 0.0f, 0.0f);

            while (tetromino.ChildBlocks[i].transform.position.x > worldPlayArea.xMax)
                tetromino.transform.position += new Vector3(-1.0f, 0.0f, 0.0f);

            while (tetromino.ChildBlocks[i].transform.position.y < worldPlayArea.yMin)
                tetromino.transform.position += new Vector3(0.0f, 1.0f, 0.0f);

            while (tetromino.ChildBlocks[i].transform.position.y > worldPlayArea.yMax)
                tetromino.transform.position += new Vector3(0.0f, -1.0f, 0.0f);
        }
    }

    private bool TryMoveTetromino(Tetromino tetromino, Vector2 moveVector)
    {
        Vector3 previousPosition = ActiveTetromino.transform.position;
        Vector3 newPosition = previousPosition + new Vector3(moveVector.x, moveVector.y, 0.0f);
        ActiveTetromino.transform.position += newPosition;
        LimitTetrominoInsidePlayArea(tetromino);

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
        ActiveTetromino.transform.rotation = Quaternion.Euler(previousEulerAngles + eulerAngles);
        LimitTetrominoInsidePlayArea(tetromino);

        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
            tetromino.ChildBlocks[i].transform.rotation = Quaternion.identity;

        Vector2[] testOffsets = SuperRotationSystem.GetTestOffsetsForDefaultTetromino(previousEulerAngles.z, eulerAngles.z);
        for (int i = 0; i < testOffsets.Length; i++)
        {
            if (TryMoveTetromino(tetromino, testOffsets[i]))
                return true;
        }

        ActiveTetromino.transform.rotation = Quaternion.Euler(previousEulerAngles);
        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
            tetromino.ChildBlocks[i].transform.rotation = Quaternion.identity;
        return false;
    }

    private void PlaceTetrominoOnPlayArea(Tetromino tetromino)
    {
        for (int i = 0; i < tetromino.ChildBlocks.Length; i++)
        {
            Vector3 pieceLocalPosition = tetromino.ChildBlocks[i].transform.position - GameArea.transform.position;

            int x = Mathf.FloorToInt(pieceLocalPosition.x);
            int y = Mathf.FloorToInt(pieceLocalPosition.y);
            Color color = tetromino.ChildBlocks[i].Color;
            GameArea.AddBlockAtPosition(x, y, color);
        }
    }
}
