using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public static class SuperRotationSystem
    {
        // Super Rotation System defined here
        // https://tetris.wiki/SRS
        private static readonly Dictionary<string, Vector2[]> _srsWallKickData = new Dictionary<string, Vector2[]>
        {
            {"[D]0->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1,  1), new Vector2(0, -2), new Vector2(-1, -2) } },
            {"[D]R->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1, -1), new Vector2(0,  2), new Vector2( 1,  2) } },
            {"[D]R->2", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1, -1), new Vector2(0,  2), new Vector2( 1,  2) } },
            {"[D]2->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1,  1), new Vector2(0, -2), new Vector2(-1, -2) } },
            {"[D]2->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1,  1), new Vector2(0, -2), new Vector2( 1, -2) } },
            {"[D]L->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0,  2), new Vector2(-1,  2) } },
            {"[D]L->0", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0,  2), new Vector2(-1,  2) } },
            {"[D]0->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1,  1), new Vector2(0, -2), new Vector2( 1, -2) } },

            {"[I]0->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-2, 0), new Vector2( 1, 0), new Vector2(-2, -1), new Vector2( 1,  2) } },
            {"[I]R->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 2, 0), new Vector2(-1, 0), new Vector2( 2,  1), new Vector2(-1, -2) } },
            {"[I]R->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2( 2, 0), new Vector2(-1,  2), new Vector2( 2, -1) } },
            {"[I]2->R", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2(-2, 0), new Vector2( 1, -2), new Vector2(-2,  1) } },
            {"[I]2->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 2, 0), new Vector2(-1, 0), new Vector2( 2,  1), new Vector2(-1, -2) } },
            {"[I]L->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-2, 0), new Vector2( 1, 0), new Vector2(-2, -1), new Vector2( 1,  2) } },
            {"[I]L->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2(-2, 0), new Vector2( 1, -2), new Vector2(-2,  1) } },
            {"[I]0->L", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2( 2, 0), new Vector2(-1,  2), new Vector2( 2, -1) } }
        };

        private static string GetRotationStateForAngle(float angleInDegrees)
        {
            int angle = Mathf.RoundToInt(angleInDegrees);
            while (angle < 0)
                angle += 360;
            angle = angle % 360;

            if (angle == 0)
                return "0";
            else if (angle == 90)
                return "L";
            else if (angle == 180)
                return "2";
            else if (angle == 270)
                return "R";
            return "?";
        }

        private static string GetPrefixForWallKickType(Tetromino.WallKickType wallKickType)
        {
            if (wallKickType == Tetromino.WallKickType.ITetrominoRotation)
                return "[I]";
            return "[D]";
        }

        public static Vector2[] GetTestOffsets(Tetromino.WallKickType wallKickType, float fromAngleInDegrees, float toAngleInDegrees)
        {
            string dictionaryKey =
                GetPrefixForWallKickType(wallKickType) + 
                GetRotationStateForAngle(fromAngleInDegrees) +
                "->" +
                GetRotationStateForAngle(toAngleInDegrees);
                
            Vector2[] testPositions;
            if (_srsWallKickData.TryGetValue(dictionaryKey, out testPositions))
                return testPositions;
            return new Vector2[] { Vector2.zero };
        }
    }
}
