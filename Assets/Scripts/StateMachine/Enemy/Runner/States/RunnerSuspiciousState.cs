using UnityEngine;
using UnityEngine.AI;

public class RunnerSuspiciousState : RunnerState
{
    private Transform _transform;
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private EnemyVision EnemyVision => EnemyInstance.EnemyVision;
    public float SuspicionLevel => EnemyVision.SuspicionLevel;

    public RunnerSuspiciousState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data,
        enemy)
    {
        _transform = enemy.Transform;
    }

    public override void Enter()
    {
        Debug.Log("Смотрю Смотрю же");
        _agent.updateRotation = false;
        _agent.isStopped = true;
    }

    public override void Update()
    {
        RotateToTarget();

        if (SuspicionLevel > Data.SuspicionToSearch)
        {
            StateSwitcher.SwitchState<RunnerSearchState>();
        }
        else if (SuspicionLevel <= 0)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
    }

    public override void Exit()
    {
        _agent.updateRotation = true;
        _agent.isStopped = false;
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition;

        if (EnemyVision.IsCurrentlySeeing)
        {
            targetPosition = EnemyVision.PlayerPosition.Transform.position;
        }
        else
        {
            targetPosition = EnemyVision.LastKnownPosition;
        }

        Vector3 direction = (targetPosition - _transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, Data.RotationSpeed * Time.deltaTime);
    }
}