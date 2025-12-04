using System;
using UnityEngine;

public class Runner : Enemy
{
    [SerializeField] private HealthData healthData;
    [SerializeField] private PatrolPoints _patrolPoints;

    private IDamageSource _damageSource;
    private EnemyHealth _health;
    private int _currentWaypoint;
    private RunnerStateMachine _stateMachine;
    
    public PatrolPoints PatrolPoints => _patrolPoints;
    public int CurrentWaypoint => _currentWaypoint;
    public IDamageSource  DamageSource => _damageSource;

    private void Awake()
    {
        _damageSource = GetComponentInChildren<DamageSource>();
        _health = new EnemyHealth(healthData);
    }

    public void SetCurrentWaypoint(int waypoint)
    {
        if (waypoint >= _patrolPoints.Waypoints.Count || waypoint < 0)
            throw new ArgumentOutOfRangeException(nameof(waypoint));
        
        _currentWaypoint = waypoint;
    }
    
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
