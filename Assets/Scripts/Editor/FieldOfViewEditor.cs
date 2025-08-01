using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewRadius);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.NearbyRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.HalfViewAngle);
        Vector3 viewAngleB = fov.DirFromAngle(fov.HalfViewAngle);
        
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);
        Handles.color = Color.red;
        
        if (fov.VisibleTarget is not null)
            Handles.DrawLine(fov.transform.position, fov.VisibleTarget.position);
    }
}

