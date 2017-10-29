using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerImpl : MonoBehaviour, GameController
{
	public GamePlayfieldImpl GamePlayfield;
	public TetrominoController TetrominoController;
	public GameMenu GameMenu;
	public MainMenu MainMenu;

	private ScoreController _scoreController;
	private GameInput _gameInput;

	private void Awake()
	{
		_scoreController = new ScoreControllerImpl();
		_gameInput = new KeyboardGameInput();
		
		TetrominoController.Initialize(GamePlayfield, _gameInput, _scoreController);
		GameMenu.Initialize(this, _scoreController);
		MainMenu.Initialize(this);
	}

    public void RestartGame()
    {
        // NOTHING HERE
    }

    public void StartGame(int width, int height)
    {		
        GamePlayfield.SetGridSize(width, height);
		_scoreController.ResetScore();
		TetrominoController.CreateRandomTetromino();
    }
}
