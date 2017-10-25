using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : TetrominoController.Input
{
	public bool MoveLeft { get { return UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow); } }
	public bool MoveRight { get { return UnityEngine.Input.GetKeyDown(KeyCode.RightArrow); } }
	public bool MoveDown { get { return UnityEngine.Input.GetKeyDown(KeyCode.DownArrow); } }
	public bool RotateClockwise { get { return UnityEngine.Input.GetKeyDown(KeyCode.Z); } }
	public bool RotateCounterClockwise { get { return UnityEngine.Input.GetKeyDown(KeyCode.X); } }
	public bool HardDrop { get  { return UnityEngine.Input.GetKeyDown(KeyCode.UpArrow); } }
}
