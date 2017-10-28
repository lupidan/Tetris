using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
	[SerializeField] private Text _scoreLabel;
	[SerializeField] private Text _highscoreLabel;

	#region MonoBehaviour

	private void OnEnable()
	{
		SetDisplayedScore(0);
		SetDisplayedHighscore(1000);
	}

	#endregion

	#region Public methods

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
		// DO STUFF
	}

	#endregion

	#region Private methods

	private void SetScoreLabel(Text label, long score)
	{
		label.text = score.ToString("0000000000");
	}

	#endregion
}
