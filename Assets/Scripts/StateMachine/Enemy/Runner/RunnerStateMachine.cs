// Убираем подписку из конструктора

using System.Collections.Generic;

public class RunnerStateMachine : StateMachine
{
    private EnemyVision _enemyVision;

    public RunnerStateMachine(Runner runner)
    {
        _enemyVision = runner.EnemyVision;

        States = new List<IState>()
        {
            new RunnerPatrolState(this, runner.Data, runner),
            new RunnerWaitingState(this, runner.Data, runner),
            new RunnerFollowState(this, runner.Data, runner),
            new RunnerAttackState(this, runner.Data, runner),
            new RunnerLookAroundState(this, runner.Data, runner),
        };

        CurrentState = States[0];
        // Подписываемся при старте, но с учётом состояния
        UpdateVisionSubscription();
        CurrentState.Enter();
    }

    ~RunnerStateMachine()
    {
        if (_enemyVision != null)
        {
            UnsubscribeFromVisionEvents();
        }
    }

    private void UpdateVisionSubscription()
    {
        UnsubscribeFromVisionEvents(); // Всегда отписываемся

        // Подписываемся в зависимости от текущего состояния
        if (CurrentState is RunnerPatrolState || CurrentState is RunnerWaitingState)
        {
            // Не подписываемся
        }
        else // Follow, LookAround, Attack
        {
            SubscribeToVisionEvents();
        }
    }

    private void SubscribeToVisionEvents()
    {
        _enemyVision.PlayerSpotted += HandlePlayerSpotted;
        _enemyVision.PlayerAlerted += HandlePlayerAlerted;
        _enemyVision.PlayerLost += HandlePlayerLost;
    }

    private void UnsubscribeFromVisionEvents()
    {
        _enemyVision.PlayerSpotted -= HandlePlayerSpotted;
        _enemyVision.PlayerAlerted -= HandlePlayerAlerted;
        _enemyVision.PlayerLost -= HandlePlayerLost;
    }

    public void SwitchState<State>()
    {
        IState state = States.Find(s => s is State);
        if (state == null) return;

        CurrentState.Exit();
        // Отписываемся от старого состояния
        UpdateVisionSubscription();

        CurrentState = state;

        // Подписываемся на новое состояние
        UpdateVisionSubscription();

        CurrentState.Enter();
    }

    private void HandlePlayerSpotted() { SwitchState<RunnerFollowState>(); }
    private void HandlePlayerAlerted() { SwitchState<RunnerLookAroundState>(); }
    private void HandlePlayerLost() { SwitchState<RunnerLookAroundState>(); }
}