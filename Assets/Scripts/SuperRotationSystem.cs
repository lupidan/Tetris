using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SuperRotationSystem
{
	// Super Rotation System defined here
	// https://tetris.wiki/SRS
	private static readonly Dictionary<string, Vector2[]> _srsDefaultWallKickData = new Dictionary<string, Vector2[]>
	{
		{"0->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1,  1), new Vector2(0, -2), new Vector2(-1, -2) } },
		{"R->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1, -1), new Vector2(0,  2), new Vector2( 1,  2) } },
		{"R->2", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1, -1), new Vector2(0,  2), new Vector2( 1,  2) } },
		{"2->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1,  1), new Vector2(0, -2), new Vector2(-1, -2) } },
		{"2->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1,  1), new Vector2(0, -2), new Vector2( 1, -2) } },
		{"L->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0,  2), new Vector2(-1,  2) } },
		{"L->0", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0,  2), new Vector2(-1,  2) } },
		{"0->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2( 1,  1), new Vector2(0, -2), new Vector2( 1, -2) } }
	};

	private static readonly Dictionary<string, Vector2[]> _srsITetrominoWallKickData = new Dictionary<string, Vector2[]>
	{
		{"0->R", new Vector2[]{ new Vector2(0, 0), new Vector2(-2, 0), new Vector2( 1, 0), new Vector2(-2, -1), new Vector2( 1,  2) } },
		{"R->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 2, 0), new Vector2(-1, 0), new Vector2( 2,  1), new Vector2(-1, -2) } },
		{"R->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2( 2, 0), new Vector2(-1,  2), new Vector2( 2, -1) } },
		{"2->R", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2(-2, 0), new Vector2( 1, -2), new Vector2(-2,  1) } },
		{"2->L", new Vector2[]{ new Vector2(0, 0), new Vector2( 2, 0), new Vector2(-1, 0), new Vector2( 2,  1), new Vector2(-1, -2) } },
		{"L->2", new Vector2[]{ new Vector2(0, 0), new Vector2(-2, 0), new Vector2( 1, 0), new Vector2(-2, -1), new Vector2( 1,  2) } },
		{"L->0", new Vector2[]{ new Vector2(0, 0), new Vector2( 1, 0), new Vector2(-2, 0), new Vector2( 1, -2), new Vector2(-2,  1) } },
		{"0->L", new Vector2[]{ new Vector2(0, 0), new Vector2(-1, 0), new Vector2( 2, 0), new Vector2(-1,  2), new Vector2( 2, -1) } }
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

	private static Vector2[] GetTestOffsets(float fromAngleInDegrees, float toAngleInDegrees, Dictionary<string, Vector2[]> data)
	{
		string dictionaryKey = GetRotationStateForAngle(from) + "->" + GetRotationStateForAngle(to);
		Vector2[] testPositions;
		if (data.TryGetValue(dictionaryKey, out testPositions))
			return testPositions;
		return new Vector2[] { Vector2.zero };
	}

	public static Vector2[] GetTestOffsetsForDefaultTetromino(float fromAngleInDegrees, float toAngleInDegrees)
	{
		return GetTestOffsets(fromAngleInDegrees, toAngleInDegrees, _srsDefaultWallKickData);
	}

	public static Vector2[] GetTestOffsetsForITetromino(float fromAngleInDegrees, float toAngleInDegrees)
	{
		return GetTestOffsets(fromAngleInDegrees, toAngleInDegrees, _srsITetrominoWallKickData);
	}
}
