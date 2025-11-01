using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataAsset/Enemy")] 
public class EnemyData : ScriptableObject
{
    public readonly float MinDistanceToTarget = 0.1f;
    public readonly float ViewDelay = 0.2f;
    public readonly float AlertThreshold = 100f;
    
    [Header("Movement")]
    [SerializeField, Range(0, 5)] private float _speed = 3.5f;
    [SerializeField, Range(5, 10)] private float _followSpeed = 7f;
    [SerializeField, Range(1, 90)] private int _rotationSpeed = 5;
    [SerializeField, Range(1, 90)] private int _turnSpeed = 5;
    private float _timeToWaitPatrolPoint;
    
    [Header("Vision")]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField, Range(0, 360)] private float _viewAngle = 90f;
    [SerializeField] private float _viewRadius = 10f;
    [SerializeField] private float _nearbyRadius = 3f;

    [SerializeField] private float _alertTime = 5f;
    [SerializeField] private float _alertDuration = 5f;
    [SerializeField] private float _suspicionTime = 2f;
    [SerializeField] private float _baseSuspicionPerSecond = 2f;
    [SerializeField] private float _suspicionDecayPerSecond = 2f;
    
    [SerializeField] private float _suspicionToSearch = 80f;
    
    
    public float Speed { get => _speed; set => _speed = Mathf.Clamp(value, 0, 5); }
    public float FollowSpeed { get => _followSpeed; set => _followSpeed = Mathf.Clamp(value, 5, 10); }
    public int RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = Mathf.Clamp(value, 1, 90); }
    public int TurnSpeed { get => _turnSpeed; set => _turnSpeed = Mathf.Clamp(value, 1, 90); }

    public LayerMask ObstacleMask => _obstacleMask;
    public LayerMask TargetMask => _targetMask;
    public float ViewAngle => _viewAngle;
    public float ViewRadius => _viewRadius;
    public float NearbyRadius => _nearbyRadius;
    public float AlertTime => _alertTime;
    public float AlertDuration =>  _alertDuration;
    public float SuspicionTime => _suspicionTime;
    public float BaseSuspicionPerSecond => _baseSuspicionPerSecond;   
    public float SuspicionDecayPerSecond => _suspicionDecayPerSecond;
    public float SuspicionToSearch => _suspicionToSearch;
}
