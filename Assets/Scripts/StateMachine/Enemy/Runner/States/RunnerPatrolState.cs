using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    private Vector3 _targetPosition;

    public Transform Transform => EnemyInstance.Transform;
    public IReadOnlyList<PatrolPoint> Waypoints => EnemyInstance.PatrolPoints.Waypoints;
    public int CurrentWaypointIndex => EnemyInstance.CurrentWaypoint;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}
    
    public override void Enter()
    {
        MoveToTarget();
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

    public override void Exit()
    {
        
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
        _targetPosition = new Vector3(Waypoints[CurrentWaypointIndex].Transform.position.x, Transform.position.y, Waypoints[CurrentWaypointIndex].Transform.position.z);
    }
}
