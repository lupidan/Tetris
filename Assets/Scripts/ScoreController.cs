
public delegate void ScoreControllerEvent<T>(T argument);

public interface ScoreController
{
	long Score { get; }
	long Highscore { get; }

	event ScoreControllerEvent<long> OnScoreUpdate;
	event ScoreControllerEvent<long> OnHighscoreUpdate;

	void UpdateScore(int[] destroyedLines, Tetromino usedTetromino);
	void ResetScore();
}
