using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerState
{
    public PlayerMovingState(IStateSwitcher stateSwitcher, PlayerData data, Player player) : base(stateSwitcher, data, player){ }

    public override void Enter()
    {
        Debug.Log("Entering PlayerMovingState");
    }
    
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
        Vector3 targetVelocity = new Vector3(direction.x * Data.Speed, 0, direction.z * Data.Speed);
        PlayerInstance.Rigidbody.velocity = Vector3.Lerp(PlayerInstance.Rigidbody.velocity, targetVelocity, 10f * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (InputIsZero())
            return;

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.y));
        PlayerInstance.Transform.rotation = Quaternion.Slerp(PlayerInstance.Transform.rotation, targetRotation, Data.RotationPerFrame);
    }
}
