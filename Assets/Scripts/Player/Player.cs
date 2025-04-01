using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    
    private PlayerStateMachine _stateMachine;
    private Rigidbody _rigidbody;
    private GameInput _input;
    private Transform _transform;
    
    public PlayerData Data => _data;
    public Rigidbody Rigidbody => _rigidbody;
    public GameInput Input => _input;
    public Transform Transform => _transform;

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
        _input = new GameInput();
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
