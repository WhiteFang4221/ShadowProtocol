using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(IStateSwitcher stateSwitcher, PlayerData data, Player player) : base(stateSwitcher, data, player){ }

    public override void Enter()
    {
        Debug.Log("Entering PlayerIdleState");
    }

    public override void Update()
    {
        if (InputIsZero() == false)
        {
            StateSwitcher.SwitchState<PlayerMovingState>();
        }
    }

    public override void Exit()
    {
        
    }
}
