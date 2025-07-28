using UnityEngine;

public class Runner : Enemy
{
    [SerializeField] private PatrolPoints _patrolPoints;
    
    private RunnerStateMachine _stateMachine;
    
    public PatrolPoints PatrolPoints => _patrolPoints;
    
    protected override void Initialize()
    {
        base.Initialize();
        _stateMachine = new RunnerStateMachine(this);
        Debug.Log(_stateMachine);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

}
