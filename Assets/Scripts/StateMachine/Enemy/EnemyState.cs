public abstract class EnemyState<TEnemy> : IState where TEnemy : Enemy
{
    protected  IStateSwitcher StateSwitcher;
    protected  EnemyConfig Config;
    protected  TEnemy EnemyInstance;

    public EnemyState(IStateSwitcher stateSwitcher, EnemyConfig config, TEnemy enemy)
    {
        StateSwitcher = stateSwitcher;
        Config = config;
        EnemyInstance = enemy;
    }
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}