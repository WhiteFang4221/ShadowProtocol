using System;
using System.Collections;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _viewDelay = 0.2f;
    [SerializeField] private float _viewAngle = 90f;
    [Inject] private IPlayerPosition _playerPosition;
    
    private Coroutine _findTargetCoroutine;
    private Coroutine _findNearbyTargetCoroutine;
    
    [field: SerializeField] public float AlertTime { get; private set; } = 5f;
    [field: SerializeField] public float ViewRadius { get; private set; }
    [field: SerializeField] public float NearbyRadius { get; private set; }
    public float HalfViewAngle => _viewAngle / 2f;
    public Transform VisibleTarget { get; private set; }

    public event Action PlayerSpotted;

    private void Start()
    {
        StartFindTargetCoroutine();
    }

    private void OnDisable()
    {
        StopFindTargetCoroutine();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckPlayerVisibility()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);

        bool isInRadius = distanceToPlayer <= ViewRadius;
        bool isInAngle = Vector3.Angle(transform.forward, dirToPlayer) <= HalfViewAngle;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _obstacleMask);

        bool canSeePlayer = isInRadius && isInAngle && hasLineOfSight;

        if (canSeePlayer)
        {
            StopNearbyLookTargetRoutine();
            FollowTarget();
        }
        else
        {
            VisibleTarget = null;
            
            if (_findNearbyTargetCoroutine is null)
            {
                StartNearbyLookTargetRoutine();
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private IEnumerator NearbyLookTarget()
    {
        float timer = 0f;

        while (timer < AlertTime)
        {
            if (IsPlayerInNearestZone())
            {
                FollowTarget();
                StopNearbyLookTargetRoutine();
            }

            yield return new WaitForSeconds(_viewDelay);
            timer += _viewDelay;
        }
    }
    
    private IEnumerator LookTarget(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CheckPlayerVisibility();
        }
    }

    private void FollowTarget()
    {
        VisibleTarget = _playerPosition.Transform;
        PlayerSpotted?.Invoke();
    }
    
    private bool IsPlayerInNearestZone()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);
        bool isInRadius = distanceToPlayer <= NearbyRadius;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _obstacleMask);

        return isInRadius && hasLineOfSight;
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
        _findTargetCoroutine = StartCoroutine(LookTarget(_viewDelay));
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