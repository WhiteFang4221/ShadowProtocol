using Reflex.Attributes; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDoorEnterable, IPlayerPosition, IHealth, IStunable
{
    [Inject] private PlayerConfig config;
    [Inject] private HealthConfig healthConfig;
    [Inject] private GameInput _input;
    
    private PlayerStateMachine _stateMachine;
    private Rigidbody _rigidbody;

    [field: SerializeField] public int CurrentHealth {get; private set;} = 0;
    public PlayerConfig Config => config;
    public Rigidbody Rigidbody => _rigidbody;
    public GameInput Input => _input;
    public Transform Transform { get; private set; }

    public List<KeyCard> KeyCards { get; private set; } = new() { KeyCard.None };
    
    public Health Health { get; private set; }
    
    private void Awake()
    {
        Health = new PlayerHealth(healthConfig);
        
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
        _input.Enable();
        Transform = transform;
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        CurrentHealth = Health.CurrentHealth;
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    public bool IsStun { get; }
    
    public void Stun()
    {
        Debug.Log("Player is Stun");
    }
}
