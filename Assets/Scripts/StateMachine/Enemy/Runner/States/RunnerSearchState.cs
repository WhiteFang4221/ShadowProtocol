using UnityEngine;
using UnityEngine.AI;

public class RunnerSearchState: RunnerState
{
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    private Vector3 _lastKnownPosition => EnemyInstance.EnemyVision.LastKnownPosition;
    
    private bool _hasReachedTarget = false;
    
    public RunnerSearchState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy)
    {
    }

    public override void Enter()
    {
        Debug.Log("Ищу Игрока");
       

        _agent.isStopped = false;
        _agent.updateRotation = true; // разрешаем поворот при движении
        _agent.SetDestination(EnemyInstance.EnemyVision.LastKnownPosition);
        _enemyVision.IsDecaySuspicion = false; // останавливаем падение подозрения
        _hasReachedTarget = false;
    }

    public override void Update()
    {
        if (_enemyVision.IsCurrentlySeeing)
        {
            _agent.SetDestination(EnemyInstance.EnemyVision.LastKnownPosition);
            
            if (_enemyVision.SuspicionLevel >= Data.AlertThreshold)
            {
                StateSwitcher.SwitchState<RunnerAlertState>();
                return;
            }
        }
        
        if (!_hasReachedTarget && _transform.position.IsEnoughClose(EnemyInstance.EnemyVision.LastKnownPosition, Data.MinDistanceToTarget))
        {
            _agent.isStopped = true;
            _hasReachedTarget = true;
            _enemyVision.IsDecaySuspicion = true;
        }
        
        if (_hasReachedTarget || _enemyVision.SuspicionLevel <= 0)
        {
            StateSwitcher.SwitchState<RunnerLookAroundState>();
        }
    }

    public override void Exit()
    {
        _agent.isStopped = false; 
        _enemyVision.IsDecaySuspicion = true;
    }

    private void MoveToTarget()
    {
        
        
    }
}
