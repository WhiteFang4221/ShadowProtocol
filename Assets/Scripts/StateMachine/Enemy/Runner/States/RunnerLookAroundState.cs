using UnityEngine;
using UnityEngine.AI;

public class RunnerLookAroundState : RunnerState
{
    private readonly float _turnAngle = 70;
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    private float _stateTimer;
    private float _initialWaitTime = 1f;
    private float _lookDuration = 1.5f;
    private float _turnSpeed = 200f;

    private float _initialAngle;
    private float _targetAngle;
    private float _currentAngle;

    private enum LookPhase { InitialWait, TurnRight, LookRight, TurnLeft, LookLeft, TurnCenter, LookCenter, Done }
    private LookPhase _currentPhase;

    public RunnerLookAroundState(IStateSwitcher stateSwitcher, EnemyConfig config, Runner enemy)
        : base(stateSwitcher, config, enemy) { }

    public override void Enter()
    {
        _agent.isStopped = true;
        _agent.updateRotation = false;

        _initialAngle = _transform.eulerAngles.y;
        _currentAngle = _initialAngle;
        _targetAngle = _initialAngle;

        _stateTimer = 0f;
        _currentPhase = LookPhase.InitialWait;
    }

    public override void Update()
    {
        if (_enemyVision.IsCurrentlySeeing)
        {
            if (_enemyVision.SuspicionLevel >= Config.AlertThreshold)
                StateSwitcher.SwitchState<RunnerAlertState>();
            else
                StateSwitcher.SwitchState<RunnerSuspiciousState>();
            return;
        }
        
        _stateTimer += Time.deltaTime;

        switch (_currentPhase)
        {
            case LookPhase.InitialWait:
                LookSide(LookPhase.TurnRight, _turnAngle);
                break;

            case LookPhase.TurnRight:
                TurnSide(LookPhase.LookRight);
                break;

            case LookPhase.LookRight:
                LookSide(LookPhase.TurnLeft, -_turnAngle);
                break;

            case LookPhase.TurnLeft:
                TurnSide(LookPhase.LookLeft);
                break;

            case LookPhase.LookLeft:
                LookSide(LookPhase.TurnCenter, 0);
                break;

            case LookPhase.TurnCenter:
                TurnSide(LookPhase.LookCenter);
                break;

            case LookPhase.LookCenter:
                if (_stateTimer >= _lookDuration)
                {
                    _currentPhase = LookPhase.Done;
                }
                break;

            case LookPhase.Done:
                StateSwitcher.SwitchState<RunnerPatrolState>();
                return;
        }
    }

    public override void Exit()
    {
        _agent.isStopped = false;
        _agent.updateRotation = true;
    }

    private void LookSide(LookPhase lookPhase, float turnAngle)
    {
        if (_stateTimer >= _initialWaitTime)
        {
            _targetAngle = _initialAngle + turnAngle;
            _currentPhase = lookPhase;
            _stateTimer = 0f;
        }
    }

    private void TurnSide(LookPhase lookPhase)
    {
        _currentAngle = Mathf.MoveTowards(_currentAngle, _targetAngle, _turnSpeed * Time.deltaTime);
        _transform.rotation = Quaternion.Euler(0f, _currentAngle, 0f);
        
        if (Mathf.Approximately(_currentAngle, _targetAngle))
        {
            _currentPhase = lookPhase;
            _stateTimer = 0f;
        }
    }
}