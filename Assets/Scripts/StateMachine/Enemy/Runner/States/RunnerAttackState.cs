using System;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAttackState : RunnerState
{
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private EnemyAnimationHandler EnemyAnimationHandler => EnemyInstance.EnemyAnimationHandler;
    
    public RunnerAttackState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        _agent.isStopped = true; 
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    private void AttemptAttack()
    {
        
    }
}
