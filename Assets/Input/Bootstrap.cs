using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private GameInput _input;


    private void Start()
    {
        _input = new GameInput();
        _input.Enable();
        _player.Initialize(_input);
    }
}
