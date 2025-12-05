using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
public abstract class Enemy : MonoBehaviour, IDoorEnterable
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private EnemyVision _enemyVision;
    [SerializeField] private List<KeyCard> _keyCards;
    [SerializeField] private EnemyAnimationHandler _enemyAnimationHandler;
    [SerializeField] private DamageSource _damageSource;
    
    private EnemyAttackHandler _attackHandler;
    private NavMeshAgent _meshAgent;
    private Transform _transform;
    public EnemyVision EnemyVision => _enemyVision;
    public EnemyData Data => _data;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _meshAgent;
    public List<KeyCard> KeyCards => _keyCards;
    public EnemyAnimationHandler EnemyAnimationHandler => _enemyAnimationHandler;

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _attackHandler = gameObject.AddComponent<EnemyAttackHandler>();
        _attackHandler.Initialize(_enemyAnimationHandler, _damageSource);
        _meshAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
    }
}