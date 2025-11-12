using UnityEngine;
using UnityEngine.AI;

public class RunnerAlertState : RunnerState
{
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    private float _remainingAlertTime;
    private Vector3 _currentChaseTarget; // Текущая цель, за которой гонимся

    public RunnerAlertState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy) { }

    public override void Enter()
    {
        Debug.Log("В ТРЕВОГЕ!");
        _agent.isStopped = false;
        _agent.updateRotation = true; // разрешаем поворот при движении

        // Начинаем преследование текущей позиции игрока
        _currentChaseTarget = _enemyVision.PlayerPosition.Transform.position;
        _agent.SetDestination(_currentChaseTarget);

        // Подозрение не падает в тревоге
        _enemyVision.IsDecaySuspicion = true;

        // Устанавливаем таймер
        _remainingAlertTime = Data.TimeSeePlayerAfterLoss; // Используем значение из Data
    }

    public override void Update()
    {
        if (_enemyVision.IsCurrentlySeeing)
        {
            // Обновляем цель на текущую позицию игрока
            _currentChaseTarget = _enemyVision.PlayerPosition.Transform.position;
            _agent.SetDestination(_currentChaseTarget);
            _remainingAlertTime = Data.TimeSeePlayerAfterLoss; // Сбрасываем таймер
        }
        else
        {
            // Не обновляем цель, продолжаем идти к _currentChaseTarget
            // Уменьшаем таймер
            _remainingAlertTime -= Time.deltaTime;

            // Если таймер истёк - переходим к поиску
            if (_remainingAlertTime <= 0)
            {
                StateSwitcher.SwitchState<RunnerSearchState>();
                return;
            }
            // Иначе продолжаем идти к _currentChaseTarget (куда шли, когда игрок исчез)
        }
    }

    public override void Exit()
    {
        _agent.isStopped = false; // на всякий случай
        _enemyVision.IsDecaySuspicion = true; // возвращаем к норме
    }
}