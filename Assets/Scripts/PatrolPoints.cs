using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private List<Vector3> _waypoints = new();
    
    public IReadOnlyList<Vector3> Waypoints => _waypoints;

    private void Awake()
    {
        AddWaypoints();
    }

    private void AddWaypoints()
    {
        _waypoints.Clear();
        
        foreach (Transform child in transform)
        {
            _waypoints.Add(child.position);
        }
    }
}
