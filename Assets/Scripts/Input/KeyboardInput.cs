using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Tetris
{
    public class KeyboardInput : Input
    {
        public bool MoveLeft { get { return UnityInput.GetKeyDown(KeyCode.LeftArrow); } }
        public bool MoveRight { get { return UnityInput.GetKeyDown(KeyCode.RightArrow); } }
        public bool MoveDown { get { return UnityInput.GetKeyDown(KeyCode.DownArrow); } }
        public bool RotateClockwise { get { return UnityInput.GetKeyDown(KeyCode.Z); } }
        public bool RotateCounterClockwise { get { return UnityInput.GetKeyDown(KeyCode.X); } }
        public bool HardDrop { get  { return UnityInput.GetKeyDown(KeyCode.UpArrow); } }
    }
}
