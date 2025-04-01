using System;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    private int _currentWaypointIndex = 0;
    
    public override void Enter()
    {
        Debug.Log("Начинаю идти");
        EnemyInstance.Agent.isStopped = false;
        EnemyInstance.Agent.speed = Data.Speed;
    }

    public override void Update()
    {
        if (Transform.position.IsEnoughClose())
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % EnemyInstance.Waypoints.Count;
            MoveToNextWaypoint();
        }
    }

    public override void Exit()
    {
        EnemyInstance.Agent.ResetPath(); // Останавливаем движение
    }

    private void MoveToNextWaypoint()
    {
        Transform targetWaypoint = EnemyInstance.Waypoints[_currentWaypointIndex];
        EnemyInstance.Agent.SetDestination(targetWaypoint.position);
    }
}
