using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    private Vector3 _targetPosition;

    public Transform Transform => EnemyInstance.Transform;
    public int CurrentWaypointIndex => EnemyInstance.CurrentWaypoint;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    

    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy){}
    
    public override void Enter()
    {
        Debug.Log("Патрулирую");
        Agent.speed = Config.Speed;
        MoveToTarget();
        EnemyInstance.EnemyVision.OnPlayerFirstSpotted += OnPlayerSpotted;
    }
    


    public override void Update()
    {
        if (Transform.position.IsEnoughClose(_targetPosition, Config.MinDistanceToTarget))
        {
            Stop();
            StateSwitcher.SwitchState<RunnerWaitingState>();
        }
    }

    public override void Exit()
    {
        EnemyInstance.EnemyVision.OnPlayerFirstSpotted -= OnPlayerSpotted;
    }

    private void OnPlayerSpotted()
    {
        StateSwitcher.SwitchState<RunnerSuspiciousState>();
    }
    
    private void MoveToTarget()
    {
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
