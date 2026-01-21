using UnityEngine;
using UnityEngine.AI;

public class RunnerSuspiciousState : RunnerState
{
    public RunnerSuspiciousState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config,
        enemy){}

    public override void Enter()
    {
        Debug.Log("Смотрю Смотрю же");
        agent.updateRotation = false;
        agent.isStopped = true;
    }

    public override void Update()
    {
        RotateToTarget();

        if (suspicionLevel > Config.SuspicionToSearch)
        {
            StateSwitcher.SwitchState<RunnerSearchState>();
        }
        else if (suspicionLevel <= 0)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
    }

    public override void Exit()
    {
        agent.updateRotation = true;
        agent.isStopped = false;
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition;

        if (enemyVision.IsCurrentlySeeing)
        {
            targetPosition = enemyVision.PlayerPosition.Transform.position;
        }
        else
        {
            targetPosition = enemyVision.LastKnownPosition;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Config.RotationSpeed * Time.deltaTime);
    }
}