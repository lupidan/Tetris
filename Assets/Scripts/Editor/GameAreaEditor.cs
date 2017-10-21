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
        Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMin, 0.0f), new Vector3(rect.xMax, rect.yMin, 0.0f));
        Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMin, 0.0f), new Vector3(rect.xMax, rect.yMax, 0.0f));
        Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMax, 0.0f), new Vector3(rect.xMin, rect.yMax, 0.0f));
        Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMax, 0.0f), new Vector3(rect.xMin, rect.yMin, 0.0f));
    }

}