using System.Collections.Generic;

public class RunnerStateMachine : StateMachine
{
    public RunnerStateMachine(Runner runner)
    {
        
        States = new List<IState>()
        {
            new RunnerPatrolState(this, runner.Data, runner),
            new RunnerSuspiciousState(this, runner.Data, runner),
            new RunnerSearchState(this, runner.Data, runner),
            new RunnerWaitingState(this, runner.Data, runner),
            new RunnerAlertState(this, runner.Data, runner),
            new RunnerAttackState(this, runner.Data, runner),
            new RunnerLookAroundState(this, runner.Data, runner),
        };
        
        CurrentState = States[0];
        CurrentState.Enter();
    }
}