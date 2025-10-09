using System;
using System.Collections.Generic;
using UnityEngine;

public class RunnerWaitingState : RunnerState
{
    private float _timerCount;
    public int CurrentWaypointIndex => EnemyInstance.CurrentWaypoint;
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
        SetNextWaypoint();
    }
    
    private void SetNextWaypoint()
    {
        int currentWaypoint;
        
        if (CurrentWaypointIndex + 1 < Waypoints.Count)
        {
            currentWaypoint = CurrentWaypointIndex + 1;
        }
        else
        {
            currentWaypoint = 0;
        }
        
        EnemyInstance.SetCurrentWaypoint(currentWaypoint);
    }
}
