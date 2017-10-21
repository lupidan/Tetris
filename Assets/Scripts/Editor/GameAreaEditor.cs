using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameArea))]
public class PlayerShipEditor : Editor
{
    [DrawGizmo(GizmoType.Selected)]
    static void DrawGameArea(GameArea gameArea, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Rect rect = gameArea.PlayArea;
        for (float x = gameArea.PlayArea.xMin; x <= gameArea.PlayArea.xMax; x += 1.0f)
            Gizmos.DrawLine(new Vector3(x, rect.yMax, 0.0f), new Vector3(x, rect.yMin, 0.0f));

        for (float y = gameArea.PlayArea.yMin; y <= gameArea.PlayArea.yMax; y += 1.0f)
            Gizmos.DrawLine(new Vector3(rect.xMin, y, 0.0f), new Vector3(rect.xMax, y, 0.0f));
    }
}
