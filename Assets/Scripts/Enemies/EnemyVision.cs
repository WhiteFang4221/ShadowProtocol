using System;
using System.Collections;
using Reflex.Attributes;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyData _enemyData;
    [Inject] private IPlayerPosition _playerPosition;
    public EnemyData Data => _enemyData;

    private float _suspicionLevel = 0f;
    private bool _isCurrentlySeeing = false;
    private bool _wasSeeingPlayer = false;
    private Vector3 _lastKnownPosition;
    private float _lastSeenTime;
    private float _lastCheckTime;
    private float _nextCheckTime;

    public bool IsCurrentlySeeing => _isCurrentlySeeing;
    public bool IsAlerted => Time.time - _lastSeenTime < _enemyData.AlertDuration;
    public Vector3 LastKnownPosition => _lastKnownPosition;
    public float SuspicionLevel => Mathf.Clamp01(_suspicionLevel) * 100f;
    
    public event Action OnPlayerFirstSpotted;
    
    private void Start()
    {
        _lastCheckTime = Time.time;
        _nextCheckTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= _nextCheckTime)
        {
            CheckVisibility();
            _nextCheckTime = Time.time + _enemyData.ViewDelay;
        }
    }

    private void CheckVisibility()
    {
        float timeSinceLastCheck = Time.time - _lastCheckTime;
        _isCurrentlySeeing = _fov.IsPlayerInField();

        if (_isCurrentlySeeing)
        {
            float distance = Vector3.Distance(transform.position, _playerPosition.Transform.position);
            float distanceFactor = 1f - Mathf.Clamp01(distance / _enemyData.ViewRadius);
            float growth = _enemyData.BaseSuspicionPerSecond * (1f + distanceFactor) * timeSinceLastCheck / 100f;
            _suspicionLevel += growth;
            
            _lastKnownPosition = _playerPosition.Transform.position;
            _lastSeenTime = Time.time;
        }
        else
        {
            _suspicionLevel -= _enemyData.SuspicionDecayPerSecond * timeSinceLastCheck / 100f;
        }
        
        if (_isCurrentlySeeing && _wasSeeingPlayer == false)
        {
            OnPlayerFirstSpotted?.Invoke();
        }
        
        _wasSeeingPlayer = _isCurrentlySeeing;
        _suspicionLevel = Mathf.Clamp01(_suspicionLevel);
    }
}

