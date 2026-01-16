using System.Collections.Generic;

public class RunnerStateMachine : StateMachine
{
    public RunnerStateMachine(Runner runner)
    {
        
        States = new List<IState>()
        {
            new RunnerPatrolState(this, runner.Config, runner),
            new RunnerSuspiciousState(this, runner.Config, runner),
            new RunnerSearchState(this, runner.Config, runner),
            new RunnerWaitingState(this, runner.Config, runner),
            new RunnerAlertState(this, runner.Config, runner),
            new RunnerAttackState(this, runner.Config, runner),
            new RunnerLookAroundState(this, runner.Config, runner),
        };
        
        CurrentState = States[0];
        CurrentState.Enter();
    }
}