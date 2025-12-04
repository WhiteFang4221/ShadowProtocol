using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyVision vision = (EnemyVision)target;
        if (vision == null || vision.Data == null)
            return;

        EnemyData data = vision.Data;
        Transform enemyTransform = vision.transform;

        float viewRadius = data.ViewRadius;
        float viewAngle = data.ViewAngle; // например, 90°
        Vector3 origin = enemyTransform.position + Vector3.up * 0.02f;
        Vector3 forward = enemyTransform.forward;

        // === Центрированный конус ===
        Vector3 arcFrom = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.up) * forward;
        Handles.color = new Color(0.2f, 0.6f, 1f, 0.25f);
        Handles.DrawSolidArc(origin, Vector3.up, arcFrom, viewAngle, viewRadius);

        // === Границы (они уже правильные) ===
        Handles.color = new Color(0.2f, 0.6f, 1f, 0.6f);
        Vector3 leftDir  = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.up) * forward;
        Vector3 rightDir = Quaternion.AngleAxis( viewAngle / 2f, Vector3.up) * forward;

        Handles.DrawLine(origin, origin + leftDir * viewRadius);
        Handles.DrawLine(origin, origin + rightDir * viewRadius);

        // === Линия к игроку (опционально) ===
        if (vision.IsCurrentlySeeing)
        {
            Handles.color = new Color(0f, 1f, 0f, 0.8f);
            Handles.DrawLine(origin, vision.LastKnownPosition);
        }
    }
}