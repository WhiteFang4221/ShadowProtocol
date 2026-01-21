using UnityEngine;
using UnityEngine.AI;

public class RunnerAlertState : RunnerState
{
    
    private float _remainingAlertTime;
    private Vector3 _currentChaseTarget => enemyVision.PlayerPosition.Transform.position; 

    public RunnerAlertState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy) { }

    public override void Enter()
    {
        Debug.Log("В ТРЕВОГЕ!");
        
        agent.isStopped = false;
        agent.SetDestination(_currentChaseTarget);
        enemyVision.IsDecaySuspicion = true;
        _remainingAlertTime = Config.TimeSeePlayerAfterLoss; 
    }

    public override void Update()
    {
        agent.SetDestination(_currentChaseTarget);
        
        if (Vector3Extensions.IsEnoughClose(agent.transform.position, _currentChaseTarget, Config.AttackRange))
        {
            StateSwitcher.SwitchState<RunnerAttackState>();
            return;
        }

        if (enemyVision.IsCurrentlySeeing)
        {
            _remainingAlertTime = Config.TimeSeePlayerAfterLoss; 
        }
        else
        {
            _remainingAlertTime -= Time.deltaTime;
            
            if (_remainingAlertTime <= 0)
            {
                StateSwitcher.SwitchState<RunnerLookAroundState>();
            }
        }
    }

    public override void Exit()
    {
        agent.isStopped = true; 
        enemyVision.IsDecaySuspicion = false; 
    }
}