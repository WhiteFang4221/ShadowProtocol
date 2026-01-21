using UnityEngine;
using UnityEngine.AI;

public class RunnerSearchState: RunnerState
{

    private Vector3 _lastKnownPosition => EnemyInstance.EnemyVision.LastKnownPosition;
    private bool _hasReachedTarget = false;
    
    public RunnerSearchState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy) : base(stateSwitcher, config, enemy)
    {
    }

    public override void Enter()
    {
        Debug.Log("Ищу Игрока");
        agent.isStopped = false;
        agent.updateRotation = true; 
        agent.SetDestination(_lastKnownPosition);
        enemyVision.IsDecaySuspicion = false;
        _hasReachedTarget = false;
    }

    public override void Update()
    {
        if (enemyVision.IsCurrentlySeeing)
        {
            agent.SetDestination(_lastKnownPosition);
            
            if (suspicionLevel >= Config.AlertThreshold)
            {
                StateSwitcher.SwitchState<RunnerAlertState>();
                return;
            }
        }
        
        if (!_hasReachedTarget && transform.position.IsEnoughClose(_lastKnownPosition, Config.MinDistanceToTarget))
        {
            agent.isStopped = true;
            _hasReachedTarget = true;
            enemyVision.IsDecaySuspicion = true;
        }
        
        if (_hasReachedTarget || suspicionLevel <= 0)
        {
            StateSwitcher.SwitchState<RunnerLookAroundState>();
        }
    }

    public override void Exit()
    {
        agent.isStopped = false; 
        enemyVision.IsDecaySuspicion = true;
    }
}
