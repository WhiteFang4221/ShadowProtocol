using Reflex.Attributes; 
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDoorEnterable, IPlayerPosition
{
    [SerializeField] private HealthData _healthData;
    [SerializeField] private PlayerData _data;
    
    private static PlayerHealth _health;
    private PlayerStateMachine _stateMachine;
    private Rigidbody _rigidbody;
    [Inject] private GameInput _input;

    [field: SerializeField] public int CurrentHealth {get; private set;} = 0;
    public PlayerData Data => _data;
    public Rigidbody Rigidbody => _rigidbody;
    public GameInput Input => _input;
    public Transform Transform { get; private set; }

    public List<KeyCard> KeyCards { get; private set; } = new() { KeyCard.None };
    
    private void Awake()
    {
        _health = new PlayerHealth(_healthData);
        
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
        _input.Enable();
        Transform = transform;
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        CurrentHealth = _health.CurrentHealth;
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    
}
