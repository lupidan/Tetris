using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Vector3 PositioningOffset { get { return _positioningOffset; } }
    public Block[] ChildBlocks { get { return _childBlocks; } }

    [SerializeField] private Vector3 _positioningOffset;
    [SerializeField] private Block[] _childBlocks;
}
