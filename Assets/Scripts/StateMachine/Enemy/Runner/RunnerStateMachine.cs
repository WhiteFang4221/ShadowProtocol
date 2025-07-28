using System.Collections.Generic;

public class RunnerStateMachine : StateMachine
{
    private FieldOfView _fieldOfView;
    public RunnerStateMachine(Runner runner)
    {
        _fieldOfView = runner.FieldOfView;
        _fieldOfView.PlayerSpotted += HandlePlayerSpotted;
        
        States = new List<IState>()
        {
            new RunnerPatrolState(this, runner.Data, runner),
            new RunnerWaitingState(this, runner.Data, runner),
            new RunnerFollowState(this, runner.Data, runner),
            new RunnerAttackState(this, runner.Data, runner),
            new RunnerLookAroundState(this, runner.Data, runner),
        };
        
        CurrentState = States[0];
        CurrentState.Enter();
    }

    private void HandlePlayerSpotted()
    {
        SwitchState<RunnerFollowState>();
    }
}