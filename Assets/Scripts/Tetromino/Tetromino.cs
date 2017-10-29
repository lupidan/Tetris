using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class Tetromino : MonoBehaviour
    {
        public enum WallKickType
        {
            DefaultRotation,
            ITetrominoRotation
        }

        public ComponentPool<Tetromino> OriginPool;

        public Vector3 PositioningOffset { get { return _positioningOffset; } }
        public WallKickType WallKick { get { return _wallKick; } }
        public Block[] ChildBlocks { get { return _childBlocks; } }

        [SerializeField] private Vector3 _positioningOffset;
        [SerializeField] private WallKickType _wallKick;
        [SerializeField] private Block[] _childBlocks;

        public void AdjustTetrominoChildBlocksRotation()
        {
            for (int i = 0; i < ChildBlocks.Length; ++i)
                ChildBlocks[i].transform.rotation = Quaternion.identity;
        }
    }
}
