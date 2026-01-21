using System;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAttackState : RunnerState
{
    private EnemyAnimationHandler EnemyAnimationHandler => EnemyInstance.EnemyAnimationHandler;

    private bool _isAttacking = false;

    public RunnerAttackState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config,
        enemy){}

    public override void Enter()
    {
        enemyVision.IsDecaySuspicion = true;
        EnemyAnimationHandler.OnAttackHitFrame += SetAttacking;
        EnemyAnimationHandler.OnAttackEndFrame += SetAttacking;
    }

    public override void Update()
    {
        RotateToTarget();
        
        if (_isAttacking == false && IsInRangeAttack())
        {
            EnemyAnimationHandler.Attack();
            _isAttacking = true;
        }
        else if (_isAttacking == false && IsInRangeAttack() == false)
        {
            StateSwitcher.SwitchState<RunnerAlertState>();
        }
    }

    public override void Exit()
    {
        agent.isStopped = false;
        enemyVision.IsDecaySuspicion = false;
        EnemyAnimationHandler.OnAttackHitFrame -= SetAttacking;
        EnemyAnimationHandler.OnAttackEndFrame -= SetAttacking;
    }

    private void SetAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    private bool IsInRangeAttack()
    {
        return Vector3Extensions.IsEnoughClose(transform.position, enemyVision.PlayerPosition.Transform.position,
            Config.AttackRange);
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition;

        targetPosition = enemyVision.PlayerPosition.Transform.position;


        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Config.RotationSpeed * Time.deltaTime);
    }
}