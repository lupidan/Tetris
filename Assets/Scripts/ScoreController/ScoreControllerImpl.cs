using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControllerImpl : ScoreController
{
    public long Score { get; private set; }

    public long Highscore { get; private set; }

    public event ScoreControllerEvent<long> OnScoreUpdate;
    public event ScoreControllerEvent<long> OnHighscoreUpdate;

    public void ResetScore()
    {
        
    }

    public void UpdateScore(int[] destroyedLines, Tetromino usedTetromino)
    {
        
    }
}
