using UnityEngine;

public class RunnerWaitingState : RunnerState
{
    private float _timerCount;
    public RunnerWaitingState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner runner) : base(stateSwitcher, config, runner){}
    
    public override void Enter()
    {
        _timerCount = 0;
        enemyVision.OnPlayerFirstSpotted += OnPlayerSpotted;
    }

    public override void Update()
    {
        if (_timerCount >= waypoints[EnemyInstance.CurrentWaypoint].WaitTime)
        {
            SetNextWaypoint();
            StateSwitcher.SwitchState<RunnerPatrolState>();
        }
        
        _timerCount += Time.deltaTime;
    }

    public override void Exit()
    {
        enemyVision.OnPlayerFirstSpotted -= OnPlayerSpotted;
    }
    
    
    private void OnPlayerSpotted()
    {
        StateSwitcher.SwitchState<RunnerSuspiciousState>();
    }
    
    private void SetNextWaypoint()
    {
        int currentWaypoint;
        
        if (EnemyInstance.CurrentWaypoint + 1 < waypoints.Count)
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
