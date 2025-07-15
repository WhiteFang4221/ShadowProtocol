using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _viewDelay = 0.2f;
    [Inject] private IPlayerPosition _playerPosition;
    
    private Coroutine _findTargetCoroutine;
    private float _halfViewAngle => ViewAngle / 2f;
    [field: SerializeField] public float ViewRadius { get; private set; }
    [field: SerializeField] public float ViewAngle { get; private set; }
    public Transform VisibleTarget { get; private set; }

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
        VisibleTarget = null;
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, _playerPosition.Transform.position) > ViewRadius) return;
        if (Vector3.Angle(transform.forward, dirToPlayer) > _halfViewAngle) return;

        if (!Physics.Raycast(transform.position, dirToPlayer, Vector3.Distance(transform.position, _playerPosition.Transform.position), _obstacleMask))
            VisibleTarget = _playerPosition.Transform;
    }
    
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private IEnumerator FindTargetWithDelay(float delay)
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
        _findTargetCoroutine = StartCoroutine(FindTargetWithDelay(_viewDelay));
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