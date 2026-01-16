using UnityEngine;

public abstract class PlayerState : IState
{
    protected readonly IStateSwitcher StateSwitcher;
    protected readonly PlayerConfig Config;
    protected readonly Player PlayerInstance;
    protected Vector2 MoveInput;

    public PlayerState(IStateSwitcher stateSwitcher, PlayerConfig config, Player player)
    {
        StateSwitcher = stateSwitcher;
        Config = config;
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
