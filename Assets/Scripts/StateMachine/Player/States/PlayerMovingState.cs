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
            return;

        Vector3 direction = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
        Vector3 targetPosition =  PlayerInstance.Rigidbody.position + direction * (Data.Speed * Time.fixedDeltaTime);
        PlayerInstance.Rigidbody.MovePosition(targetPosition);
    }

    private void Rotate()
    {
        if (InputIsZero())
            return;

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.y));
        PlayerInstance.Transform.rotation = Quaternion.Slerp(PlayerInstance.Transform.rotation, targetRotation, Data.RotationPerFrame);
    }
}
