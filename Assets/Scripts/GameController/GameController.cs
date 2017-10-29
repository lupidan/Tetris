namespace Tetris
{
    public interface GameController
    {
        void StartGame(int width, int height);
        void QuitGame();
        void RestartGame();
        void GameOver();
    }
}
