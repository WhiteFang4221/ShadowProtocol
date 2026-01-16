using UnityEngine;

public class PlayerStunState : PlayerState
{
    public PlayerStunState(IStateSwitcher stateSwitcher, PlayerConfig config, Player player) : base(stateSwitcher, config, player){ }
    
    public override void Enter()
    {
    }

    public override void HandleInput()
    {
        MoveInput = Vector2.zero;
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}
