using System;
using Reflex.Attributes;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyData _enemyData;
    [Inject] private IPlayerPosition _playerPosition;
    public EnemyData Data => _enemyData;

    [SerializeField] private float _suspicionLevel = 0f;
    private bool _isCurrentlySeeing = false;
    private bool _wasSeeingPlayer = false;
    private Vector3 _lastKnownPosition;
    private float _lastSeenTime;
    private float _lastCheckTime;
    private float _nextCheckTime;
    private bool _isDecaySuspicion = true;


    public IPlayerPosition PlayerPosition => _playerPosition;
    public bool IsCurrentlySeeing => _isCurrentlySeeing;
    public bool IsAlerted => Time.time - _lastSeenTime < _enemyData.AlertDuration;
    public Vector3 LastKnownPosition => _lastKnownPosition;
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
            // Вычисляем половину радиуса
            float halfRadius = _enemyData.ViewRadius / 2f;

            float distanceFactor = _enemyData.ViewRadius > 0 ? 1f - Mathf.Clamp01(distance / _enemyData.ViewRadius) : 0;

            // Вычисляем базовый рост
            float growth = _enemyData.BaseSuspicionPerSecond * (1f + distanceFactor) * timeSinceLastCheck / 100f;

            // Если игрок ближе половины радиуса - ускоряем в 4 раза
            if (distance < halfRadius)
            {
                growth *= 4f;
            }

            _suspicionLevel += growth;

            _lastKnownPosition = _playerPosition.Transform.position;
            _lastSeenTime = Time.time;
        }
        else
        {
            if (IsDecaySuspicion)
            {
                // Используем значение из EnemyData
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

