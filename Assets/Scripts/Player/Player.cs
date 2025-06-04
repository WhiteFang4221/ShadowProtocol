using Reflex.Attributes; 
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDoorEnterable
{
    [SerializeField] private PlayerData _data;
    
    private PlayerStateMachine _stateMachine;
    private Rigidbody _rigidbody;
    [Inject] private GameInput _input;
    private Transform _transform;
    
    public PlayerData Data => _data;
    public Rigidbody Rigidbody => _rigidbody;
    public GameInput Input => _input;
    public Transform Transform => _transform;

    public List<KeyCard> KeyCards { get; private set; } = new() { KeyCard.None };
    
    private void Awake()
    {
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
        _input.Enable();
        _transform = transform;
    }

    private void Update()
    {
        _stateMachine.HandleInput();
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

}
