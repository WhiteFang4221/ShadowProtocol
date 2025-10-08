using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private List<PatrolPoint> _waypoints = new();
    
    public IReadOnlyList<PatrolPoint> Waypoints => _waypoints;

    private void Awake()
    {
        AddWaypoints();
    }

    private void AddWaypoints()
    {
        _waypoints.Clear();
        
        foreach (Transform child in transform)
        {
           if(child.gameObject.TryGetComponent(out PatrolPoint patrolPoint))
                _waypoints.Add(patrolPoint);
        }
    }
}
