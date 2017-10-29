namespace Tetris
{
    public interface Input
    {
        bool MoveLeft { get; }
        bool MoveRight { get; }
        bool MoveDown { get; }
        bool RotateClockwise { get; }
        bool RotateCounterClockwise { get; }
        bool HardDrop { get; }
    }
}
