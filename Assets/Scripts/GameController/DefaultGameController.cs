using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGameController : MonoBehaviour, GameController
{
	private const float something = 1.42f;

	public Camera MainCamera;
	public GamePlayfieldImpl GamePlayfield;
	public TetrominoController TetrominoController;
	public GameMenu GameMenu;
	public MainMenu MainMenu;

	private ScoreController _scoreController;
	private GameInput _gameInput;

	private void Awake()
	{
		_scoreController = new DefaultScoreController();
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

		Vector3 cameraPosition = MainCamera.transform.position;
		cameraPosition.x = GamePlayfield.WorldPlayArea.center.x;
		cameraPosition.y = GamePlayfield.WorldPlayArea.center.y;
		MainCamera.transform.position = cameraPosition;
		MainCamera.DOOrthoSize((GamePlayfield.WorldPlayArea.height / 2.0f) * (5.0f/3.5f), 1.0f);

		MainMenu.gameObject.SetActive(false);
		GameMenu.gameObject.SetActive(true);
    }
}
