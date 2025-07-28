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
    [SerializeField] private float _alertTime = 5f;
    
    [Inject] private IPlayerPosition _playerPosition;

    private bool _isAlerted = false;
    
    private Coroutine _findTargetCoroutine;
    private float _halfViewAngle => ViewAngle / 2f;
    [field: SerializeField] public float ViewRadius { get; private set; }
    [field: SerializeField] public float AlertRadius { get; private set; }
    [field: SerializeField] public float ViewAngle { get; private set; }
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
    
    private void CheckPlayerVisibility()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);
        
        bool isInRadius = distanceToPlayer <= ViewRadius;
        bool isInAngle = Vector3.Angle(transform.forward, dirToPlayer) <= _halfViewAngle;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _obstacleMask);

        bool canSeePlayer = isInRadius && isInAngle && hasLineOfSight;

        if (canSeePlayer)
        {
            VisibleTarget = _playerPosition.Transform;
            PlayerSpotted?.Invoke();
        }
        else
        {
            if (_isAlerted == false)
            {
                StartCoroutine(AlertLookTarget());
            }
        }
        
    }
    
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private IEnumerator AlertLookTarget()
    {
        _isAlerted = true;
        float timer = 0f;

        while (timer < _alertTime)
        {
            yield return new WaitForSeconds(_viewDelay);
            timer += _viewDelay;

            if (CheckPlayerInAlertZone())
            {
                VisibleTarget = _playerPosition.Transform;
                PlayerSpotted?.Invoke();
                yield break;
            }
        }

        VisibleTarget = null;
        _isAlerted = false;
    }
    
    private bool CheckPlayerInAlertZone()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);
        bool isInRadius = distanceToPlayer <= AlertRadius;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _obstacleMask);

        return isInRadius && hasLineOfSight;
    }
    
    private IEnumerator LookTarget(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CheckPlayerVisibility();
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