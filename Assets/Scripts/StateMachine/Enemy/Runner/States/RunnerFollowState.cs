using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerFollowState : RunnerState
{
    private Vector3 _lastTargetPosition;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    public RunnerFollowState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
       Agent.speed = Data.FollowSpeed;
    }

    public override void Update()
    {

    }



    public override void Exit()
    {

    }

    private void MoveToTarget()
    {

    }
    
}
