using System.Collections;
using UnityEngine;

public class RunnerLookAroundState: RunnerState
{
    private Coroutine _lookAroundRoutine;
    private Coroutine _timerAlertRoutine;
    private WaitForSeconds _lookAroundDelay = new WaitForSeconds(0.5f);
    private float _rotationAngle = 60f;
    private bool _isStateFinished = false;
    public int RotationSpeed => Data.RotationSpeed;
    
    public RunnerLookAroundState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        StartLookAroundCoroutine();
    }

    public override void Update()
    {
        // Нет необходимости в Update, вся логика в корутинах.
        // RunnerStateMachine переключит состояние при событиях PlayerSpotted/PlayerAlerted.
    }

    public override void Exit()
    {
        // Очень важно остановить ВСЕ корутины при выходе из состояния
        StopLookAroundCoroutine();
        StopTimerRoutine();
        // Сбрасываем флаг, на всякий случай, если состояние будет повторно использоваться
        _isStateFinished = false;
    }

    private IEnumerator LookAroundRoutine()
    {
        StartTimerRoutine(); // Запускаем таймер поиска
        Quaternion originalRotation = EnemyInstance.Transform.rotation;
        
        // Основной цикл поворотов
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

        // Цикл завершён (по таймеру или ручной остановке через _isStateFinished)
        // Останавливаем таймер (на всякий случай, хотя он уже должен был завершиться)
        StopTimerRoutine();
        _isStateFinished = false; // Сбрасываем флаг для следующего входа
        // Переключаемся обратно в патруль
        StateSwitcher.SwitchState<RunnerPatrolState>();
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        // Плавный поворот
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
        // Ждём AlertTime
        yield return new WaitForSeconds(EnemyInstance.Data.AlertTime);
        // Устанавливаем флаг, чтобы завершить LookAroundRoutine
        _isStateFinished = true;
    }

    private void StartLookAroundCoroutine()
    {
        StopLookAroundCoroutine(); // Останавливаем, если уже запущена
        _lookAroundRoutine = EnemyInstance.StartCoroutine(LookAroundRoutine());
    }

    private void StopLookAroundCoroutine()
    {
        if (_lookAroundRoutine != null)
        {
            EnemyInstance.StopCoroutine(_lookAroundRoutine);
            _lookAroundRoutine = null;
        }
    }
    
    private void StartTimerRoutine()
    {
        StopTimerRoutine(); // Останавливаем, если уже запущена
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