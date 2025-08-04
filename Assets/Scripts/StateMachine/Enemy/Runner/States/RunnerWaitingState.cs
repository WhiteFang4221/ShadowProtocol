using System;
using UnityEngine;

public class RunnerWaitingState : RunnerState
{
    private float _timerCount = 0;
    public RunnerWaitingState(IStateSwitcher stateSwitcher, EnemyData data, Runner runner) : base(stateSwitcher, data, runner){}
    
    public override void Enter()
    {
        _timerCount = 0;
    }

    public override void Update()
    {
        _timerCount += Time.deltaTime;

        if (_timerCount >= Data.TimeToWaitPatrolPoint)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
    }

    public override void Exit()
    {
        
    }
    
}
