using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
public abstract class Enemy : MonoBehaviour, IDoorEnterable, IHealth, IStunable
{
    [FormerlySerializedAs("_data")] [SerializeField] private EnemyConfig config;
    [FormerlySerializedAs("_healthData")] [SerializeField] private HealthConfig healthConfig;
    [SerializeField] private EnemyVision _enemyVision;
    [SerializeField] private List<KeyCard> _keyCards;
    [SerializeField] private EnemyAnimationHandler _enemyAnimationHandler;
    
    private NavMeshAgent _meshAgent;
    private Transform _transform;
    public EnemyVision EnemyVision => _enemyVision;
    public EnemyConfig Config => config;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _meshAgent;
    public List<KeyCard> KeyCards => _keyCards;
    public EnemyAnimationHandler EnemyAnimationHandler => _enemyAnimationHandler;
    public Health Health { get; private set; }

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        Health = new PlayerHealth(healthConfig);
    }

    public bool IsStun { get; }
    public void Stun()
    {
        Debug.Log("Enemy is Stun");
    }
}