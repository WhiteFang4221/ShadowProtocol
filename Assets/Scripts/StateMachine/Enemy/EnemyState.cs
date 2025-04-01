public abstract class EnemyState<TEnemy> : IState where TEnemy : Enemy
{
    protected  IStateSwitcher StateSwitcher;
    protected  EnemyData Data;
    protected  TEnemy EnemyInstance;

    public EnemyState(IStateSwitcher stateSwitcher, EnemyData data, TEnemy enemy)
    {
        StateSwitcher = stateSwitcher;
        Data = data;
        EnemyInstance = enemy;
    }
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}