using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    [SerializeField] private List<Transform> _waypoints;
    
    private RunnerStateMachine _stateMachine;

    public List<Transform> Waypoints => _waypoints;
    
    protected override void Initialize()
    {
        base.Initialize();
        _stateMachine = new RunnerStateMachine(this);
    }
}
