using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
    public class GameMenu : MonoBehaviour
    {
        public Text GameOverLabel { get { return _gameOverLabel; } }

        [SerializeField] private Text _gameOverLabel;
        [SerializeField] private Text _scoreLabel;
        [SerializeField] private Text _highscoreLabel;
        [SerializeField] private Text _timerLabel;

        private GameController _gameController;
        private ScoreController _scoreController;

        #region MonoBehaviour
        
        private void Start()
        {
            _scoreController.OnScoreUpdate += SetDisplayedScore;
            _scoreController.OnHighscoreUpdate += SetDisplayedHighscore;
            _gameController.OnGameTimeUpdate += SetDisplayedGameTime;

            SetDisplayedScore(_scoreController.Score);
            SetDisplayedHighscore(_scoreController.Highscore);
            SetDisplayedGameTime(_gameController.GameTime);
        }

        private void OnDestroy()
        {
            _scoreController.OnScoreUpdate -= SetDisplayedScore;
            _scoreController.OnHighscoreUpdate -= SetDisplayedHighscore;
            _gameController.OnGameTimeUpdate -= SetDisplayedGameTime;
        }

        #endregion

        #region Public methods

        public void Initialize(GameController gameController, ScoreController scoreController)
        {
            this._gameController = gameController;
            this._scoreController = scoreController;
        }

        public void SetDisplayedScore(long score)
        {
            SetScoreLabel(_scoreLabel, score);
        }

        public void SetDisplayedHighscore(long highscore)
        {
            SetScoreLabel(_highscoreLabel, highscore);
        }

        public void SetDisplayedGameTime(TimeSpan gameTime)
        {
            _timerLabel.text = gameTime.Minutes.ToString("00") + ":" + gameTime.Seconds.ToString("00");
        }

        #endregion

        #region Button actions

        public void RestartButtonWasSelected()
        {
            _gameController.RestartGame();
            _gameOverLabel.gameObject.SetActive(false);
        }

        public void QuitButtonWasSelected()
        {
            _gameController.QuitGame();
            _gameOverLabel.gameObject.SetActive(false);
        }

        #endregion

        #region Private methods

        private void SetScoreLabel(Text label, long score)
        {
            label.text = score.ToString("00000000");
        }

        #endregion
    }
}
