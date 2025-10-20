using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Reflex.Attributes; 

public class RunnerFollowState : RunnerState
{
    private Vector3 _lastKnownTargetPosition;
    private float _timeSinceLastSighting; // Время с последнего "свидания" (видения или тревоги)
    
    public Transform Transform => EnemyInstance.Transform;
    public NavMeshAgent Agent => EnemyInstance.Agent;
    // Предполагаем, что EnemyInstance.EnemyVision предоставляет нужные методы
    // и что IPlayerPosition инжектится туда или доступен через DI напрямую Runner'у.
    // Для простоты, будем использовать Vision для проверки видимости и получения позиции игрока через DI.
    public FieldOfView Vision => EnemyInstance.EnemyVision.GetComponent<FieldOfView>(); // Получаем FieldOfView
    [Inject] private IPlayerPosition _playerPosition; // Инжектим позицию игрока напрямую Runner'у (предполагая, что Reflex может это сделать)

    public RunnerFollowState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy)
    {
        // Предполагаем, что Inject произойдёт автоматически через Reflex при создании экземпляра,
        // или что Runner передаёт IPlayerPosition в конструктор RunnerState.
        // Если Inject не сработает, можно передавать IPlayerPosition через конструктор RunnerState.
    }

    public override void Enter()
    {
        Agent.speed = Data.FollowSpeed;
        Agent.isStopped = false; // Убедимся, что агент не остановлен

        // Получаем текущую позицию игрока как "последнюю известную" при входе в состояние
        _lastKnownTargetPosition = _playerPosition.Transform.position;
        _timeSinceLastSighting = 0f;

        // Начинаем движение к текущей позиции
        MoveToTarget();
    }

    public override void Update()
    {
        // Проверяем, видим ли игрок прямо сейчас
        if (Vision.IsPlayerInField())
        {
            // Игрок видим, обновляем "последнюю известную позицию" и движемся к нему
            _lastKnownTargetPosition = _playerPosition.Transform.position;
            _timeSinceLastSighting = 0f; // Сбрасываем таймер

            // Если мы находимся близко к игроку, возможно, нужно переключиться в AttackState
            // (предполагаем, что MinDistanceToTarget в EnemyData или где-то рядом)
            float distanceToTarget = Vector3.Distance(Transform.position, _lastKnownTargetPosition);
            if (distanceToTarget <= Data.MinDistanceToTarget)
            {
                // StateSwitcher.SwitchState<RunnerAttackState>(); // Если реализовано
                // Или просто продолжаем следовать, если атака не требуется сразу
            }

            // Всё равно вызываем SetDestination, чтобы поддерживать движение к текущей позиции
            MoveToTarget();
        }
        else // Игрок НЕ видим
        {
            // Обновляем таймер с последнего "свидания"
            _timeSinceLastSighting += Time.deltaTime;

            // Проверяем, может ли агент достичь последней известной позиции
            if (Agent.hasPath && Agent.remainingDistance <= Data.MinDistanceToTarget)
            {
                // Достигли последней известной позиции, останавливаемся
                Agent.isStopped = true;
                // Дальнейшее поведение зависит от логики. Враг может начать "осматриваться" на месте.
                // Однако, основное переключение (в LookAround) происходит через событие PlayerLost в RunnerStateMachine.
                // Можно добавить таймер ожидания на месте, если игрок не появляется.
                // Но для простоты, оставим агента остановленным и ждём событий от RunnerStateMachine.
                // Если игрок снова появляется в поле зрения, Enter вызовется снова.
            }
            else if (!Agent.hasPath || Agent.pathPending)
            {
                // Если нет пути или путь в ожидании, пробуем установить путь к последней известной позиции
                // Это может случиться, если путь был прерван или ещё не установлен при входе.
                // Проверим, что _lastKnownTargetPosition не равен текущей позиции, чтобы избежать ошибки NavMesh
                if (Vector3.Distance(Transform.position, _lastKnownTargetPosition) > Data.MinDistanceToTarget)
                {
                     MoveToTarget();
                }
            }
            // Если путь есть и не достигнут, агент продолжает двигаться к _lastKnownTargetPosition
        }
    }

    public override void Exit()
    {
        Agent.isStopped = true; // Останавливаем агента при выходе из состояния
        Agent.ResetPath(); // Сбрасываем путь, чтобы избежать дрейфа
    }

    private void MoveToTarget()
    {
        // Убеждаемся, что агент не остановлен перед установкой пути
        Agent.isStopped = false;
        Agent.SetDestination(_lastKnownTargetPosition);
    }
}