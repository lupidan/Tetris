using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultScoreController : ScoreController
{
    public long Score { get; private set; }
    public long Highscore { get; private set; }

    public event ScoreControllerEvent<long> OnScoreUpdate;
    public event ScoreControllerEvent<long> OnHighscoreUpdate;

    public void ResetScore()
    {
        Score = 0;

        if (OnScoreUpdate != null)
            OnScoreUpdate(Score);
    }

    public void UpdateScore(int[] destroyedLines, Tetromino usedTetromino)
    {
        switch (destroyedLines.Length)
        {
            case 1: Score += 40; break;
            case 2: Score += 100; break;
            case 3: Score += 300; break;
            case 4: Score += 1200; break;
        }

        if (OnScoreUpdate != null)
            OnScoreUpdate(Score);

        if (Score > Highscore)
        {
            Highscore = Score;

            if (OnHighscoreUpdate != null)
                OnHighscoreUpdate(Highscore = Score);
        }
    }
}
