using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolState : RunnerState
{
    public RunnerPatrolState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}
    
    public override void Enter()
    {
    }


    public override void Update()
    {
    }

    public override void Exit(){}

    private void MoveToTarget()
    {

    }

}
