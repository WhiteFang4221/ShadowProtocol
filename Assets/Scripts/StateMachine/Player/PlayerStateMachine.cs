using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(Player player)
    {
        States = new List<IState>
        {
            new PlayerIdleState(this, player.Data, player),
            new PlayerMovingState(this, player.Data, player),
            new PlayerStunState(this, player.Data, player),
        };

        CurrentState = States[0];
        CurrentState.Enter();
    }

}
