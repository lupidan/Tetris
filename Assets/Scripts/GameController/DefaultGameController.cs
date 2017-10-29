using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class DefaultGameController : MonoBehaviour, GameController
    {
        private const float PlayfieldToVisibleRatio = 1.42f;

        [Header("Menus")]
        public MainMenu MainMenu;
        public GameMenu GameMenu;
        [Header("Game elements")]
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
            // NOTHING HERE
        }
    }
}
