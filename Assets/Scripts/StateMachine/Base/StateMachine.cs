using System.Collections.Generic;
using System.Linq;

public abstract class StateMachine : IStateSwitcher
{
    protected List<IState> States = new();
    protected IState CurrentState;

    public void Update()
    {
        CurrentState.Update();
    }
    
    public void SwitchState<State>() where State : IState
    {
        IState state = States.FirstOrDefault(state => state is State);

        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }
}
