using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _rotationPerFrame = 0.2f;
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
        Rotate();
        Move();
    }

    private void HandleInput()
    {
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (InputIsZero())
            return;

        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
        Vector3 targetPosition = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    private void Rotate()
    {
        if (InputIsZero())
            return;

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_moveInput.x, 0, _moveInput.y));
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _rotationPerFrame);
    }

    private bool InputIsZero()
    {
        return _moveInput == Vector2.zero;
    }
}