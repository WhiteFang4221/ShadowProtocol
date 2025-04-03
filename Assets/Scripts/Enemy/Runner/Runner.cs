using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Runner : Enemy
{
    [SerializeField] private PatrolPoints _patrolPoints;
    
    private RunnerStateMachine _stateMachine;

    public PatrolPoints PatrolPoints => _patrolPoints;
    public bool HasAcces { get; }
    
    protected override void Initialize()
    {
        base.Initialize();
        _stateMachine = new RunnerStateMachine(this);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

}
