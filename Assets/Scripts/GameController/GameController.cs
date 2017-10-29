﻿namespace Tetris
{
    public interface GameController
    {
        void StartGame(int width, int height);
        void RestartGame();
        void EndGame();
        void QuitGame();
    }
}
