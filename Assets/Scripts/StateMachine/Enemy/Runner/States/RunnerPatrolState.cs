using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    private int _currentWaypointIndex;
    private Vector3 _targetPosition;

    public Transform Transform => EnemyInstance.Transform;
    public IReadOnlyList<PatrolPoint> Waypoints => EnemyInstance.PatrolPoints.Waypoints;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}
    
    public override void Enter()
    {
        MoveToTarget();
        EnemyInstance.Data.TimeToWaitPatrolPoint = Waypoints[_currentWaypointIndex].WaitTime;
    }


    public override void Update()
    {
        if (Transform.position.IsEnoughClose(_targetPosition, Data.MinDistanceToTarget))
        {
            Stop();
            SetNextWaypoint();
            StateSwitcher.SwitchState<RunnerWaitingState>();
        }
    }

    public override void Exit(){}

    
    private void SetNextWaypoint()
    {
        if (_currentWaypointIndex + 1 < Waypoints.Count)
        {
            _currentWaypointIndex++;
        }
        else
        {
            _currentWaypointIndex = 0;
        }
    }

    private void MoveToTarget()
    {
        Agent.speed = Data.Speed;
        Agent.isStopped = false;
        SetTargetPosition();
        Agent.SetDestination(_targetPosition);
    }

    private void Stop()
    {
        Agent.isStopped = true;
    }
    
    private void SetTargetPosition()
    {
        _targetPosition = new Vector3(Waypoints[_currentWaypointIndex].Transform.position.x, Transform.position.y, Waypoints[_currentWaypointIndex].Transform.position.z);
    }
}
