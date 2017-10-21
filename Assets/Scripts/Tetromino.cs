using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private static float GridSize = 1.0f;
    private static float HardDropMultiplier = 10.0f;
    private static float RotationAngleInDegrees = 90.0f;

    public Transform[] ChildBlocks { get { return _childBlocks;} }

    [SerializeField] private Transform[] _childBlocks;

    public void MoveRight()
    {
        Move(GridSize, 0.0f);
    }

    public void MoveLeft()
    {
        Move(-GridSize, 0.0f);
    }

    public void MoveDown()
    {
        Move(0.0f, -GridSize);
    }

    public void MoveUp()
    {
        Move(0.0f, GridSize);
    }

    public void HardDrop()
    {
        Move(0.0f, -GridSize * HardDropMultiplier);
    }

    public void RotateClockwise()
    {
        Rotate(-RotationAngleInDegrees);
    }

    public void RotateCounterClockwise()
    {
        Rotate(RotationAngleInDegrees);
    }

    private void Move(float xIncrement, float yIncrement)
    {
        Vector3 newPosition = this.transform.position;
        newPosition.x += xIncrement;
        newPosition.y += yIncrement;
        this.transform.position = newPosition;
    }

    private void Rotate(float angleIncrementInDegrees)
    {
        Vector3 newEulerRotation = this.transform.rotation.eulerAngles;
        newEulerRotation.z += angleIncrementInDegrees;
        this.transform.rotation = Quaternion.Euler(newEulerRotation);

        for (int i=0; i < this._childBlocks.Length; ++i)
            this._childBlocks[i].localEulerAngles = -newEulerRotation;
    }
}
