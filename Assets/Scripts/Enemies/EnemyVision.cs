using System;
using Reflex.Attributes;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyData _enemyData;
    [Inject] private IPlayerPosition _playerPosition;

    [SerializeField] private float _suspicionLevel = 0f;
    private bool _isCurrentlySeeing = false;
    private bool _wasSeeingPlayer = false;
    private Vector3 _lastKnownPosition;
    private float _lastSeenTime;
    private float _lastCheckTime;
    private float _nextCheckTime;
    private bool _isDecaySuspicion = true;

    public EnemyData Data => _enemyData;
    public IPlayerPosition PlayerPosition => _playerPosition;
    public bool IsCurrentlySeeing => _isCurrentlySeeing;
    public bool IsAlerted => Time.time - _lastSeenTime < _enemyData.AlertDuration;

    public Vector3 LastKnownPosition
    {
        get => _lastKnownPosition;
        set
        {
            if (Mathf.Abs(value.y - 1f) > Mathf.Epsilon)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Y-координата LastKnownPosition должна быть равна 1. Получено: {value.y}");
            }

            _lastKnownPosition = value;
        }
    }

    public float SuspicionLevel => _suspicionLevel * 100f;

    public bool IsDecaySuspicion
    {
        get => _isDecaySuspicion;
        set => _isDecaySuspicion = value;
    }

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
            float normalizedDistance = Mathf.Clamp01(distance / _enemyData.ViewRadius);
            float distanceFactor = 1f - normalizedDistance;
            float distanceMultiplier = 1f + (distanceFactor * distanceFactor) * _enemyData.DistanceInfluenceFactor;
            float growth = _enemyData.BaseSuspicionPerSecond * distanceMultiplier * timeSinceLastCheck / 100f;

            _suspicionLevel += growth;
            _lastKnownPosition = _playerPosition.Transform.position;
            _lastSeenTime = Time.time;
        }
        else
        {
            if (IsDecaySuspicion == false)
            {
                _suspicionLevel -= _enemyData.SuspicionDecayPerSecond * timeSinceLastCheck / 100f;
            }
        }

        if (_isCurrentlySeeing && _wasSeeingPlayer == false)
        {
            OnPlayerFirstSpotted?.Invoke();
        }

        _wasSeeingPlayer = _isCurrentlySeeing;
        _suspicionLevel = Mathf.Clamp01(_suspicionLevel);
        _lastCheckTime = Time.time;
    }
}