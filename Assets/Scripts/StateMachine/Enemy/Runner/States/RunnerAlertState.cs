using UnityEngine;
using UnityEngine.AI;

public class RunnerAlertState : RunnerState
{
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    private float _remainingAlertTime;
    private Vector3 _currentChaseTarget; 

    public RunnerAlertState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy) { }

    public override void Enter()
    {
        Debug.Log("В ТРЕВОГЕ!");
        
        _agent.isStopped = false;
        _currentChaseTarget = _enemyVision.PlayerPosition.Transform.position;
        _agent.SetDestination(_currentChaseTarget);
        _enemyVision.IsDecaySuspicion = true;
        _remainingAlertTime = Config.TimeSeePlayerAfterLoss; 
    }

    public override void Update()
    {
        _currentChaseTarget = _enemyVision.PlayerPosition.Transform.position;
        _agent.SetDestination(_currentChaseTarget);
        
        if (Vector3Extensions.IsEnoughClose(_agent.transform.position, _currentChaseTarget, Config.AttackRange))
        {
            StateSwitcher.SwitchState<RunnerAttackState>();
            return;
        }

        if (_enemyVision.IsCurrentlySeeing)
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
        _agent.isStopped = true; 
        _enemyVision.IsDecaySuspicion = false; 
    }
}