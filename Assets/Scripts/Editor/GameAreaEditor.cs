using UnityEngine;
using UnityEditor;
using Tetris;

[CustomEditor(typeof(DefaultPlayfield))]
public class PlayerShipEditor : Editor
{
    [DrawGizmo(GizmoType.Selected)]
    static void DrawPlayfieldArea(DefaultPlayfield playfield, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Rect rect = playfield.WorldPlayArea;
        for (float x = rect.xMin; x <= rect.xMax; x += 1.0f)
            Gizmos.DrawLine(new Vector3(x, rect.yMax, 0.0f), new Vector3(x, rect.yMin, 0.0f));

        for (float y = rect.yMin; y <= rect.yMax; y += 1.0f)
            Gizmos.DrawLine(new Vector3(rect.xMin, y, 0.0f), new Vector3(rect.xMax, y, 0.0f));
    }
}
