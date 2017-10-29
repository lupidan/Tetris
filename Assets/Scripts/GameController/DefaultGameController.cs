using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class DefaultGameController : MonoBehaviour, GameController
    {
        private const float PlayfieldToVisibleRatio = 1.5f;

        [Header("Menus")]
        public MainMenu MainMenu;
        public GameMenu GameMenu;
        [Header("Game elements")]
        public Camera MainCamera;
        public DefaultPlayfield Playfield;
        public DefaultTetrominoSpawner TetrominoSpawner;
        public TetrominoController TetrominoController;
        
        private ScoreController _scoreController;
        private Input _gameInput;

        #region MonoBehaviour methods

        private void Awake()
        {
            _scoreController = new DefaultScoreController();
            _gameInput = new KeyboardInput();
            
            TetrominoController.Initialize(this, Playfield, TetrominoSpawner, _gameInput, _scoreController);
            GameMenu.Initialize(this, _scoreController);
            MainMenu.Initialize(this);

            MainMenu.gameObject.SetActive(true);
            GameMenu.gameObject.SetActive(false);
        }

        private void Update()
        {
            GameTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
            if (OnGameTimeUpdate != null)
                OnGameTimeUpdate(GameTime);
        }

        #endregion

        #region GameController implementation

        public TimeSpan GameTime { get; private set; }

        public event GameControllerEvent<TimeSpan> OnGameTimeUpdate;

        public void StartGame(int width, int height)
        {
            Playfield.SetGridSize(width, height);
            TetrominoController.Run();
            _scoreController.ResetScore();

            MainMenu.gameObject.SetActive(false);
            GameMenu.gameObject.SetActive(true);
            GameMenu.GameOverLabel.gameObject.SetActive(false);

            Rect playfieldArea = Playfield.WorldPlayArea;
            Vector3 cameraPosition = MainCamera.transform.position;
            cameraPosition.x = playfieldArea.center.x;
            cameraPosition.y = playfieldArea.center.y;
            MainCamera.transform.position = cameraPosition;
            MainCamera.orthographicSize = (playfieldArea.height / 2.0f) * PlayfieldToVisibleRatio;

            GameTime = TimeSpan.Zero;
        }

        public void QuitGame()
        {
            Playfield.ClearGrid();
            TetrominoController.Stop();
            MainMenu.gameObject.SetActive(true);
            GameMenu.gameObject.SetActive(false);
        }

        public void RestartGame()
        {
            int width = Playfield.Width;
            int height = Playfield.Height;
            QuitGame();
            StartGame(width, height);
        }

        public void GameOver()
        {
            MainCamera.DOShakePosition(1.0f, 1.0f);
            GameMenu.GameOverLabel.gameObject.SetActive(true);
        }

        #endregion
    }
}
