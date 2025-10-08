using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private EnemyVision _enemyVision;
    
    private NavMeshAgent _meshAgent;
    private Transform _transform;
    
    public EnemyVision EnemyVision => _enemyVision;
    public EnemyData Data => _data;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _meshAgent;

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
    }
}
