using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameInput _input;


    private void Start()
    {
        _input = new GameInput();
        _input.Enable();

    }
}
