using System;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAttackState : RunnerState
{
    private float _attackTimer;
    private float _attackCooldown = 0.5f; // Время между "атаками", или время ожидания после анимации

    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;
    private IDamageSource DamageSource => EnemyInstance.DamageSource;
    private NavMeshAgent _agent => EnemyInstance.Agent;
    
    public RunnerAttackState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        _agent.isStopped = true; 
        _attackTimer = 0f;
        AttemptAttack();
    }

    public override void Update()
    {
        _attackTimer += Time.deltaTime;
        
        if (_attackTimer >= _attackCooldown)
        {
            AttemptAttack();
        }
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    private void AttemptAttack()
    {
        
    }
}
