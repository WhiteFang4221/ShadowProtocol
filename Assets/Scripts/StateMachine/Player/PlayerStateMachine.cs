using System.Collections.Generic;

public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(Player player)
    {
        States = new List<IState>
        {
            new PlayerIdleState(this, player.Config, player),
            new PlayerMovingState(this, player.Config, player),
            new PlayerStunState(this, player.Config, player),
        };
        
        CurrentState = States[0];
        CurrentState.Enter();
    }

    public void HandleInput()
    {
        ((PlayerState)CurrentState).HandleInput();
    }

}
