using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerFollowState : RunnerState
{
    private Vector3 _lastTargetPosition;
    
    public Transform Transform => EnemyInstance.Transform;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    public Transform VisibleTarget => EnemyInstance.EnemyVision.VisibleTarget;
    public RunnerFollowState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
       Agent.speed = Data.FollowSpeed;
    }

    public override void Update()
    {
        if (VisibleTarget is not null)
        {
            MoveToTarget();
        }

        if (Transform.position.IsEnoughClose(_lastTargetPosition, Data.MinDistanceToTarget))
        {
            if (VisibleTarget is null)
            {
                StateSwitcher.SwitchState<RunnerLookAroundState>();
            }
            else
            {
                StateSwitcher.SwitchState<RunnerAttackState>();
            }
        }
    }



    public override void Exit()
    {

    }

    private void MoveToTarget()
    {
        _lastTargetPosition = EnemyInstance.EnemyVision.VisibleTarget.position;
        Agent.SetDestination(_lastTargetPosition);
    }
    
}
