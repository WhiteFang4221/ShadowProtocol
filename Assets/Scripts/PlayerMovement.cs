using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private GameInput _input;
    private Transform _transform;
    private Vector3 _moveDirection;
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

    public void Initialize(GameInput input)
    {
        _input = input ?? throw new ArgumentNullException(nameof(input));
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void FixedUpdate()
    {
        HandleInput();
        
        if (InputIsZero())
            return;
        
        Move();
    }

    private bool InputIsZero()
    {
        return _moveInput == Vector2.zero;
    }
    
    private void Move()
    {
        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;
        
        
        _moveDirection = (forward * _moveInput.y + right * _moveInput.x) * _speed;
        _rigidbody.velocity = new Vector3(_moveDirection.x, 0, _moveDirection.z);
    }

    private void Rotate()
    {
        
    }
    
    private void HandleInput()
    {
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
    }
    
    
}
