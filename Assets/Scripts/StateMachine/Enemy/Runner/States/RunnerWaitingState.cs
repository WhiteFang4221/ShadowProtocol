using UnityEngine;

public class RunnerWaitingState : RunnerState
{
    private float _timerCount;
    public RunnerWaitingState(IStateSwitcher stateSwitcher, EnemyData data, Runner runner) : base(stateSwitcher, data, runner){}
    
    public override void Enter()
    {
        _timerCount = 0;
        EnemyInstance.EnemyVision.OnPlayerFirstSpotted += OnPlayerSpotted;
    }

    public override void Update()
    {
        if (_timerCount >= Waypoints[EnemyInstance.CurrentWaypoint].WaitTime)
        {
            SetNextWaypoint();
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
        
        _timerCount += Time.deltaTime;
    }

    public override void Exit()
    {
        EnemyInstance.EnemyVision.OnPlayerFirstSpotted -= OnPlayerSpotted;
    }
    
    
    private void OnPlayerSpotted()
    {
        StateSwitcher.SwitchState<RunnerSuspiciousState>();
    }
    
    private void SetNextWaypoint()
    {
        int currentWaypoint;
        
        if (EnemyInstance.CurrentWaypoint + 1 < Waypoints.Count)
        {
            currentWaypoint = EnemyInstance.CurrentWaypoint + 1;
        }
        else
        {
            currentWaypoint = 0;
        }
        
        EnemyInstance.SetCurrentWaypoint(currentWaypoint);
    }
}
