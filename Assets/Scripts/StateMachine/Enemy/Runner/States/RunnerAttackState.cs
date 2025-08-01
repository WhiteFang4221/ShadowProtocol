using System;
using UnityEngine;

public class RunnerAttackState : RunnerState
{
    public RunnerAttackState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        Debug.Log("Атакую");
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}
