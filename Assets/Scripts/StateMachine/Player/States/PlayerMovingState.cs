using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerState
{
    public PlayerMovingState(IStateSwitcher stateSwitcher, PlayerConfig config, Player player) : base(stateSwitcher, config, player){ }

    public override void Enter(){}
    
    public override void Update()
    {
        if (InputIsZero())
        {
            StateSwitcher.SwitchState<PlayerIdleState>();
        }
        
        Rotate();
        Move();
    }

    public override void Exit(){}
    
    private void Move()
    {
        if (InputIsZero())
        {
            PlayerInstance.Rigidbody.velocity = Vector3.zero;
            return;
        }
        
        Vector3 direction = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
        Vector3 targetVelocity = new Vector3(direction.x * Config.Speed, 0, direction.z * Config.Speed);
        PlayerInstance.Rigidbody.velocity = Vector3.Lerp(PlayerInstance.Rigidbody.velocity, targetVelocity, 10f * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (InputIsZero())
            return;

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.y));
        PlayerInstance.Transform.rotation = Quaternion.Slerp(PlayerInstance.Transform.rotation, targetRotation, Config.RotationPerFrame);
    }
}
