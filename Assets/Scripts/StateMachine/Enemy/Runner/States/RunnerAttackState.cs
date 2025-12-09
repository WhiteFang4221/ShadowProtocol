using System;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAttackState : RunnerState
{
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private EnemyAnimationHandler EnemyAnimationHandler => EnemyInstance.EnemyAnimationHandler;

    private bool _isAttacking = false;

    public RunnerAttackState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data,
        enemy){}

    public override void Enter()
    {
        _enemyVision.IsDecaySuspicion = true;
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
        _agent.isStopped = false;
        _enemyVision.IsDecaySuspicion = false;
        EnemyAnimationHandler.OnAttackHitFrame -= SetAttacking;
        EnemyAnimationHandler.OnAttackEndFrame -= SetAttacking;
    }

    private void SetAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    private bool IsInRangeAttack()
    {
        return Vector3Extensions.IsEnoughClose(_transform.position, _enemyVision.PlayerPosition.Transform.position,
            Data.AttackRange);
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition;

        targetPosition = _enemyVision.PlayerPosition.Transform.position;


        Vector3 direction = (targetPosition - _transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, Data.RotationSpeed * Time.deltaTime);
    }
}