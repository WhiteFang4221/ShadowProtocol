using System;
using System.Collections;
using Reflex.Attributes;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyData _enemyData;
    [Inject] private IPlayerPosition _playerPosition;
    private bool _isTargetFollow;
    
    private Coroutine _findNearbyTargetCoroutine;
    private Coroutine _findTargetCoroutine;
    public Transform VisibleTarget { get; private set; } 
    
    public EnemyData Data => _enemyData;


}

