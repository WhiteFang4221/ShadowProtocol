using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataAsset/Enemy")] 
public class EnemyData : ScriptableObject
{
    public readonly float MinDistanceToTarget = 0.1f;
    public readonly float ViewDelay = 0.1f;
    public readonly float AlertThreshold = 100f;
    
    [Header("Movement")]
    [SerializeField, Range(0, 5)] private float _speed = 3.5f;
    [SerializeField, Range(5, 10)] private float _followSpeed = 7f;
    [SerializeField, Range(1, 90)] private int _rotationSpeed = 5;
    private float _timeToWaitPatrolPoint;
    
    [Header("Vision")]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField, Range(0, 360)] private float _viewAngle = 90f;
    [SerializeField] private float _viewRadius = 10f;
    [SerializeField] private float _nearbyRadius = 3f;
    
    [SerializeField] private float _alertDuration = 5f;
    [SerializeField] private float _timeSeePlayerAfterLoss = 5f;
    [SerializeField] private float _baseSuspicionPerSecond = 20f;
    [SerializeField] private float _suspicionDecayPerSecond = 25f;
    
    [SerializeField] private float _suspicionToSearch = 50f;
    
    public float DistanceInfluenceFactor = 15f;
    public float Speed { get => _speed; set => _speed = Mathf.Clamp(value, 0, 5); }
    public float FollowSpeed { get => _followSpeed; set => _followSpeed = Mathf.Clamp(value, 5, 10); }
    public int RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = Mathf.Clamp(value, 1, 90); }

    public LayerMask ObstacleMask => _obstacleMask;
    public float ViewAngle => _viewAngle;
    public float ViewRadius => _viewRadius;
    public float AlertDuration =>  _alertDuration;
    public float BaseSuspicionPerSecond => _baseSuspicionPerSecond;   
    public float SuspicionDecayPerSecond => _suspicionDecayPerSecond;
    public float SuspicionToSearch => _suspicionToSearch;
    public float TimeSeePlayerAfterLoss => _timeSeePlayerAfterLoss;
}
