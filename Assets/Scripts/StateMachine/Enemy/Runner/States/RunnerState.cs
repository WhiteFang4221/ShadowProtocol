using System.Collections.Generic;
using UnityEngine;


public abstract class RunnerState : EnemyState<Runner>
{
    protected Transform transform;
    protected IReadOnlyList<PatrolPoint> waypoints => EnemyInstance.PatrolPoints.Waypoints;


    public RunnerState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy)
    {
        transform = enemy.Transform;
    }


    public override void Enter()
    {
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}