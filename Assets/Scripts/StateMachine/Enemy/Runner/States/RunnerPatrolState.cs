using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    private Vector3 _targetPosition;
    public int CurrentWaypointIndex => EnemyInstance.CurrentWaypoint;
    
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy){}
    
    public override void Enter()
    {
        Debug.Log("Патрулирую");
        agent.speed = Config.Speed;
        MoveToTarget();
        enemyVision.OnPlayerFirstSpotted += OnPlayerSpotted;
    }
    
    public override void Update()
    {
        if (transform.position.IsEnoughClose(_targetPosition, Config.MinDistanceToTarget))
        {
            Stop();
            StateSwitcher.SwitchState<RunnerWaitingState>();
        }
    }

    public override void Exit()
    {
        enemyVision.OnPlayerFirstSpotted -= OnPlayerSpotted;
    }

    private void OnPlayerSpotted()
    {
        StateSwitcher.SwitchState<RunnerSuspiciousState>();
    }
    
    private void MoveToTarget()
    {
        agent.isStopped = false;
        SetTargetPosition();
        agent.SetDestination(_targetPosition);
    }

    private void Stop()
    {
        agent.isStopped = true;
    }
    
    private void SetTargetPosition()
    {
        _targetPosition = new Vector3(waypoints[CurrentWaypointIndex].transform.position.x, transform.position.y, waypoints[CurrentWaypointIndex].transform.position.z);
    }
    
}
