using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class DefaultGameController : MonoBehaviour, GameController
	{
		private const float PlayfieldToVisibleRatio = 1.42f;

		public MainMenu MainMenu;
		public GameMenu GameMenu;
		public Camera MainCamera;
		public DefaultPlayfield Playfield;
		public TetrominoController TetrominoController;
		
		private ScoreController _scoreController;
		private Input _gameInput;

		private void Awake()
		{
			_scoreController = new DefaultScoreController();
			_gameInput = new KeyboardInput();
			
			TetrominoController.Initialize(Playfield, _gameInput, _scoreController);
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
			MainCamera.DOOrthoSize((GamePlayfield.WorldPlayArea.height / 2.0f) * PlayfieldToVisibleRatio, 1.0f);

			MainMenu.gameObject.SetActive(false);
			GameMenu.gameObject.SetActive(true);
		}
	}
}
