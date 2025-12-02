using UnityEngine;
using UnityEngine.AI;

public class RunnerLookAroundState : RunnerState
{
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    // Общий таймер осмотра
    private float _lookAroundTimer;
    private float _lookAroundTime => 2f; // например, 2f

    // Поворот
    private float _initialAngle; // угол, куда смотрел при входе
    private float _targetLookOffset; // целевой сдвиг от initialAngle (в градусах)
    private float _currentLookOffset; // текущий сдвиг от initialAngle (в градусах)

    // Фазы поворота
    private enum LookPhase { TurnRight, TurnLeft, ReturnCenter };
    private LookPhase _currentPhase = LookPhase.TurnRight;

    public RunnerLookAroundState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy)
        : base(stateSwitcher, data, enemy) { }

    public override void Enter()
    {
        Debug.Log("Оглядываюсь...");
        _agent.isStopped = true;
        _agent.updateRotation = false; // отключаем NavMesh-поворот

        // Сохраняем начальный угол (в градусах)
        _initialAngle = _transform.eulerAngles.y;

        // Начинаем с поворота вправо
        _currentPhase = LookPhase.TurnRight;
        _lookAroundTimer = 0f;

        // Устанавливаем целевой сдвиг для текущей фазы
        _targetLookOffset = GetTargetOffsetForPhase(_currentPhase);
        _currentLookOffset = 0f; // начнём с центра
    }

    public override void Update()
    {
        // Если игрок снова появился - срочно реагируем
        if (_enemyVision.IsCurrentlySeeing)
        {
            if (_enemyVision.SuspicionLevel >= Data.AlertThreshold)
            {
                StateSwitcher.SwitchState<RunnerAlertState>();
            }
            else
            {
                StateSwitcher.SwitchState<RunnerSuspiciousState>();
            }
            return;
        }

        // Если подозрение упало до 0 - возвращаемся к патрулю
        if (_enemyVision.SuspicionLevel <= 0)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
            return;
        }

        // Обновляем общий таймер осмотра
        _lookAroundTimer += Time.deltaTime;

        // Обновляем фазу поворота и проверяем, нужно ли переключаться
        UpdateLookPhase();

        // Если общий таймер истёк - возвращаемся к патрулю
        if (_lookAroundTimer >= _lookAroundTime)
        {
            StateSwitcher.SwitchState<RunnerPatrolState>();
            return;
        }
    }

    private void UpdateLookPhase()
    {
        // Плавно поворачиваемся к целевому смещению
        _currentLookOffset = Mathf.MoveTowards(_currentLookOffset, _targetLookOffset, Data.RotationSpeed * Time.deltaTime);

        // Применяем поворот
        float targetAngle = _initialAngle + _currentLookOffset;
        _transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        // Проверяем, достигли ли целевого смещения для текущей фазы
        if (Mathf.Approximately(_currentLookOffset, _targetLookOffset))
        {
            // Переходим к следующей фазе
            switch (_currentPhase)
            {
                case LookPhase.TurnRight:
                    _currentPhase = LookPhase.TurnLeft;
                    break;
                case LookPhase.TurnLeft:
                    _currentPhase = LookPhase.ReturnCenter;
                    break;
                case LookPhase.ReturnCenter:
                    // Если вернулись в центр, завершаем цикл поворотов
                    // Переход в другое состояние будет по таймеру _lookAroundTimer
                    return; // выходим, чтобы не менять _targetLookOffset
            }

            // Устанавливаем новый целевой сдвиг для следующей фазы
            _targetLookOffset = GetTargetOffsetForPhase(_currentPhase);
        }
    }

    private float GetTargetOffsetForPhase(LookPhase phase)
    {
        switch (phase)
        {
            case LookPhase.TurnRight:
                return 60; // +60
            case LookPhase.TurnLeft:
                return -60; // -60
            case LookPhase.ReturnCenter:
                return 0f; // возврат к 0
            default:
                return 0f;
        }
    }

    public override void Exit()
    {
        _agent.isStopped = false;
        _agent.updateRotation = true;
    }
}