using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Block[] ChildBlocks { get { return _childBlocks; } }

    [SerializeField] private Block[] _childBlocks;
}
