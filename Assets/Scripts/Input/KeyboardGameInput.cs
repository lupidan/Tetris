using UnityEngine;

public class KeyboardGameInput : GameInput
{
	public bool MoveLeft { get { return Input.GetKeyDown(KeyCode.LeftArrow); } }
	public bool MoveRight { get { return Input.GetKeyDown(KeyCode.RightArrow); } }
	public bool MoveDown { get { return Input.GetKeyDown(KeyCode.DownArrow); } }
	public bool RotateClockwise { get { return Input.GetKeyDown(KeyCode.Z); } }
	public bool RotateCounterClockwise { get { return Input.GetKeyDown(KeyCode.X); } }
	public bool HardDrop { get  { return Input.GetKeyDown(KeyCode.UpArrow); } }
}
