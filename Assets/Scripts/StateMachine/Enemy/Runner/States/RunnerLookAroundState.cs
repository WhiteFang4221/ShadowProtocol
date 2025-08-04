using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerLookAroundState: RunnerState
{
    private Coroutine _lookAroundRoutine;
    private Coroutine _timerAlertRoutine;
    private WaitForSeconds _lookAroundDelay = new WaitForSeconds(0.5f);
    private float _rotationAngle = 60f;
    private bool _isStateFinished = false;
    public int RotationSpeed => Data.RotationSpeed;
    public Transform VisibleTarget => EnemyInstance.EnemyVision.VisibleTarget;
    
    public RunnerLookAroundState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        StartLookAroundCoroutine();
    }

    public override void Update()
    {
        if (VisibleTarget is not null)
        {
            StateSwitcher.SwitchState<RunnerFollowState>();
        }
    }

    public override void Exit()
    {
        StopLookAroundCoroutine();
    }

    private IEnumerator LookAroundRoutine()
    {
        StartTimerRoutine();
        Quaternion originalRotation = EnemyInstance.Transform.rotation;
        
        while (!_isStateFinished)
        {
            Quaternion leftRotation = Quaternion.Euler(0, -_rotationAngle, 0) * originalRotation;
            Quaternion rightRotation = Quaternion.Euler(0, _rotationAngle, 0) * originalRotation;

            yield return _lookAroundDelay;
            yield return RotateTo(leftRotation);
            yield return _lookAroundDelay;
            yield return RotateTo(rightRotation);
            yield return _lookAroundDelay;
        }

        _isStateFinished = false;
        StateSwitcher.SwitchState<RunnerPatrolState>();
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        while (Quaternion.Angle(EnemyInstance.Transform.rotation, targetRotation) > 0.5f)
        {
            EnemyInstance.Transform.rotation = Quaternion.RotateTowards(
                EnemyInstance.Transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
    
    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(EnemyInstance.Data.AlertTime);
        _isStateFinished = true;
    }

    private void StartLookAroundCoroutine()
    {
        StopLookAroundCoroutine();
        _lookAroundRoutine = EnemyInstance.StartCoroutine(LookAroundRoutine());
    }

    private void StopLookAroundCoroutine()
    {
        if (_lookAroundRoutine is not null)
        {
            EnemyInstance.StopCoroutine(_lookAroundRoutine);
            _lookAroundRoutine = null;
        }
    }
    
    private void StartTimerRoutine()
    {
        StopTimerRoutine();
        _timerAlertRoutine = EnemyInstance.StartCoroutine(TimerRoutine());
    }

    private void StopTimerRoutine()
    {
        if (_timerAlertRoutine != null)
        {
            EnemyInstance.StopCoroutine(_timerAlertRoutine);
            _timerAlertRoutine = null;
        }
    }
}