using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(IStateSwitcher stateSwitcher, PlayerConfig config, Player player) : base(stateSwitcher, config, player){ }

    public override void Enter(){}

    public override void Update()
    {
        if (InputIsZero() == false)
        {
            StateSwitcher.SwitchState<PlayerMovingState>();
        }
        else
        {
            PlayerInstance.Rigidbody.velocity = Vector3.Lerp(PlayerInstance.Rigidbody.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
        }
    }

    public override void Exit(){}
}
