using DG.Tweening;
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
        }

        private void Start()
        {
            MainMenu.gameObject.SetActive(true);
            GameMenu.gameObject.SetActive(false);
        }

        #endregion

        #region GameController implementation

        public void StartGame(int width, int height)
        {		
            Playfield.SetGridSize(width, height);
            _scoreController.ResetScore();
            TetrominoController.CreateRandomTetromino();

            Rect playfieldArea = Playfield.WorldPlayArea;
            Vector3 cameraPosition = MainCamera.transform.position;
            cameraPosition.x = playfieldArea.center.x;
            cameraPosition.y = playfieldArea.center.y;
            MainCamera.transform.position = cameraPosition;
            MainCamera.DOOrthoSize((playfieldArea.height / 2.0f) * PlayfieldToVisibleRatio, 1.0f);

            MainMenu.gameObject.SetActive(false);
            GameMenu.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            StartGame(Playfield.Width, Playfield.Height);
        }

        public void EndGame()
        {
            MainCamera.DOShakePosition(1.0f, 1.0f);
            GameMenu.GameOverLabel.gameObject.SetActive(true);
        }

        public void QuitGame()
        {
            //Nothing yet
        }

        #endregion
    }
}
