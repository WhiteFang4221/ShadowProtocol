using Reflex.Attributes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyVision enemyVision = (EnemyVision)target;
        EnemyData data = enemyVision.Data;
        if (data == null)
            return;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyVision.transform.position, Vector3.up, Vector3.forward, 360, data.ViewRadius);
        Handles.DrawWireArc(enemyVision.transform.position, Vector3.up, Vector3.forward, 360, data.NearbyRadius);
        Vector3 viewAngleA = DirFromAngle(-data.ViewAngle / 2, enemyVision);
        Vector3 viewAngleB = DirFromAngle(data.ViewAngle / 2, enemyVision);
        
        Handles.DrawLine(enemyVision.transform.position, enemyVision.transform.position + viewAngleA * data.ViewRadius);
        Handles.DrawLine(enemyVision.transform.position, enemyVision.transform.position + viewAngleB * data.ViewRadius);
        Handles.color = Color.red;
        
        if (enemyVision.VisibleTarget is not null)
            Handles.DrawLine(enemyVision.transform.position, enemyVision.VisibleTarget.position);
    }
    
    private Vector3 DirFromAngle(float angleInDegrees, EnemyVision vision)
    {
        angleInDegrees += vision.transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

