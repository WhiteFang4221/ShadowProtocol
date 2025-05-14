using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IDoorEnterable
{
    [SerializeField] private EnemyData _data;
    private NavMeshAgent _meshAgent;
    private Transform _transform;
    
    public EnemyData Data => _data;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _meshAgent;
    
    public List<KeyCard> KeyCards { get; } = new List<KeyCard>() { KeyCard.None };

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
