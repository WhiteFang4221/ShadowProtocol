using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    private int _currentWaypointIndex = 0;
    private Vector3 _targetPosition;

    public Transform Transform => EnemyInstance.Transform;
    public IReadOnlyList<Vector3> Waypoints => EnemyInstance.PatrolPoints.Waypoints;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}
    
    public override void Enter()
    {
        MoveToTarget();
        Debug.Log($"Иду к точке {_targetPosition}");
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

    private void SetTargetPosition()
    {
        _targetPosition = new Vector3(Waypoints[_currentWaypointIndex].x, Transform.position.y, Waypoints[_currentWaypointIndex].z);
    }
    
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
}
