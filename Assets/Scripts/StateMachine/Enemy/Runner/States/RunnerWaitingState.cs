using System;
using System.Collections.Generic;
using UnityEngine;

public class RunnerWaitingState : RunnerState
{
    private float _timerCount = 0;
    
    public IReadOnlyList <PatrolPoint> Waypoints => EnemyInstance.PatrolPoints.Waypoints;
    
    public RunnerWaitingState(IStateSwitcher stateSwitcher, EnemyData data, Runner runner) : base(stateSwitcher, data, runner){}
        
    
    public override void Enter()
    {
        _timerCount = 0;
    }

    public override void Update()
    {
        if (_timerCount >= Waypoints[EnemyInstance.CurrentWaypoint].WaitTime)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
        
        _timerCount += Time.deltaTime;
    }

    public override void Exit()
    {
        
    }
    
}
