using System.Collections.Generic;

public class RunnerState : EnemyState<Runner>
{
    public IReadOnlyList<PatrolPoint> Waypoints => EnemyInstance.PatrolPoints.Waypoints;

    public RunnerState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy)
    {
        
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