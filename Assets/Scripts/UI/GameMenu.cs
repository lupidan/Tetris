using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
	[SerializeField] private Text _scoreLabel;
	[SerializeField] private Text _highscoreLabel;

	private IGameController _gameController;
	private ScoreController _scoreController;

	#region MonoBehaviour

	private void OnEnable()
	{
		SetDisplayedScore(0);
		SetDisplayedHighscore(1000);
	}

	private void Start()
	{
		_scoreController.OnScoreUpdate += SetDisplayedScore;
		_scoreController.OnHighscoreUpdate += SetDisplayedHighscore;
	}

	private void OnDestroy()
	{
		_scoreController.OnScoreUpdate -= SetDisplayedScore;
		_scoreController.OnHighscoreUpdate -= SetDisplayedHighscore;
	}

	#endregion

	#region Public methods

	public void Initialize(IGameController gameController, ScoreController scoreController)
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

	#endregion

	#region Button actions

	public void RestartButtonWasSelected()
	{
		_gameController.RestartGame();
	}

	#endregion

	#region Private methods

	private void SetScoreLabel(Text label, long score)
	{
		label.text = score.ToString("00000000");
	}

	#endregion
}
