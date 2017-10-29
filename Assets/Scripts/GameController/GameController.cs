using System;

namespace Tetris
{
    public delegate void GameControllerEvent<T>(T argument);

    public interface GameController
    {
        TimeSpan GameTime { get; }

        event GameControllerEvent<TimeSpan> OnGameTimeUpdate;

        void StartGame(int width, int height);
        void QuitGame();
        void RestartGame();
        void GameOver();
    }
}
