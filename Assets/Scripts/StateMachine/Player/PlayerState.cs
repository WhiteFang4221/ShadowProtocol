using UnityEngine;

public abstract class PlayerState : IState
{
    protected readonly IStateSwitcher StateSwitcher;
    protected readonly PlayerData Data;
    protected readonly Player PlayerInstance;
    protected Vector2 MoveInput;

    public PlayerState(IStateSwitcher stateSwitcher, PlayerData data, Player player)
    {
        StateSwitcher = stateSwitcher;
        Data = data;
        PlayerInstance = player;
    }
    
    public abstract void Enter();

    public virtual void HandleInput()
    {
        MoveInput = PlayerInstance.Input.Gameplay.Move.ReadValue<Vector2>();
    }
    
    public abstract void Update();
    
    public abstract void Exit();
    
    protected bool InputIsZero()
    {
        return MoveInput == Vector2.zero;
    }
}
