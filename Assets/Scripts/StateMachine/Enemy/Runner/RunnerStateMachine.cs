using System.Collections.Generic;

public class RunnerStateMachine : StateMachine
{
    public RunnerStateMachine(Runner runner)
    {
        States = new List<IState>()
        {
            new RunnerPatrolState(this, runner.Data, runner),
            new RunnerIdleState(this, runner.Data, runner),
            new RunnerFollowState(this, runner.Data, runner),
            new RunnerAttackState(this, runner.Data, runner),
        };
        
        CurrentState = States[0];
        CurrentState.Enter();
    }
}