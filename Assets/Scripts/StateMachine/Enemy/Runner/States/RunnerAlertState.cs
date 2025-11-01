using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAlertState : RunnerState
{
    private NavMeshAgent _agent => EnemyInstance.Agent;
    private Transform _transform => EnemyInstance.Transform;
    private EnemyVision _enemyVision => EnemyInstance.EnemyVision;

    private float _remainingAlertTime; // сколько времени осталось "помнить" точную позицию
    public RunnerAlertState(IStateSwitcher stateSwitcher, EnemyData data, Runner enemy) : base(stateSwitcher, data, enemy){}

    public override void Enter()
    {
        _agent.speed = Data.FollowSpeed;
       Debug.Log("Бегу за игроком");
       
       UpdateTargetDestination();

       // Подозрение не падает в тревоге
       _enemyVision.IsDecaySuspicion = true;

       // Устанавливаем таймер
       _remainingAlertTime = 3f;
    }

    public override void Update()
    {
        if (_enemyVision.IsCurrentlySeeing)
        {
            UpdateTargetDestination();
            _remainingAlertTime = 3f;
        }
        else
        {
            // Если не видим - уменьшаем таймер
            _remainingAlertTime -= Time.deltaTime;

            // Если таймер истёк - переходим к поиску
            if (_remainingAlertTime <= 0)
            {
                StateSwitcher.SwitchState<RunnerSearchState>();
                return;
            }
            // Иначе продолжаем идти к последней известной позиции
            // Цель уже установлена на _enemyVision.LastKnownPosition в прошлом вызове UpdateTargetDestination
        }
    }



    public override void Exit()
    {
        _agent.isStopped = false; // на всякий случай
        _enemyVision.IsDecaySuspicion = true; 
    }

    private void UpdateTargetDestination()
    {
        // Цель - текущая позиция игрока, если видим, иначе - последняя известная
        Vector3 targetPos = _enemyVision.IsCurrentlySeeing 
            ? _enemyVision.PlayerPosition.Transform.position 
            : _enemyVision.LastKnownPosition;

        _agent.SetDestination(targetPos);
    }
    
}
