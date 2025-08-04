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

    private WaitForSeconds _viewDelay => new WaitForSeconds(_enemyData.ViewDelay);
    public Transform VisibleTarget { get; private set; } 
    
    public event Action PlayerSpotted;
    public EnemyData Data => _enemyData;

    private void Start()
    {
        StartFindTargetCoroutine();
    }
    private void OnDisable()
    {
        StopFindTargetCoroutine();
    }

    private IEnumerator LookTarget()
    {
        while (true)
        {
            yield return _viewDelay;
            
            if (_fov.IsPlayerInField())
            {
                FollowTarget();
            }
            else
            {
                LoseTarget();
            }
        }
    }
    
    private IEnumerator NearbyLookTarget()
    {
        float timer = 0f;

        while (timer < _enemyData.AlertTime)
        {
            if (_fov.IsPlayerInNearestZone())
            {
                FollowTarget();
                StopNearbyLookTargetRoutine();
            }
            
            yield return _viewDelay; 
            timer += _enemyData.ViewDelay;
        }
    }
    
    private void FollowTarget()
    {
        VisibleTarget = _playerPosition.Transform;
        _isTargetFollow = true;
        PlayerSpotted?.Invoke();
    }

    private void LoseTarget()
    {
        VisibleTarget = null;
        
        if (_isTargetFollow)
        {
            _isTargetFollow = false;
            StartNearbyLookTargetRoutine();
        }
    }


    private void StartNearbyLookTargetRoutine()
    {
        StopNearbyLookTargetRoutine();
        _findNearbyTargetCoroutine = StartCoroutine(NearbyLookTarget());
    }

    private void StopNearbyLookTargetRoutine()
    {
        if (_findNearbyTargetCoroutine is not null)
        {
            StopCoroutine(_findNearbyTargetCoroutine);
            _findNearbyTargetCoroutine = null;
        }
    }

    private void StartFindTargetCoroutine()
    {
        StopFindTargetCoroutine();
        _findTargetCoroutine = StartCoroutine(LookTarget());
    }

    private void StopFindTargetCoroutine()
    {
        if (_findTargetCoroutine != null)
        {
            StopCoroutine(_findTargetCoroutine);
            _findTargetCoroutine = null;
        }
    }
}

