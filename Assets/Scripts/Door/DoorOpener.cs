using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private const float OpenSpeed = 8f;
    private const float OpenDistance = 1f;
    
    [SerializeField] private DoorDetector _doorDetector;
    [SerializeField] private Rigidbody _leftDoorRigidbody;
    [SerializeField] private Rigidbody _rightDoorRigidbody;

    private Vector3 _leftDoorStartPos;
    private Vector3 _rightDoorStartPos;
    private Vector3 _leftTargetPos;
    private Vector3 _rightTargetPos;
    
    private Coroutine _openCoroutine;
    private Coroutine _closeCoroutine;

    private void Start()
    {
        _leftDoorStartPos = _leftDoorRigidbody.position;
        _rightDoorStartPos = _rightDoorRigidbody.position;
        _leftTargetPos = _leftDoorRigidbody.transform.TransformPoint(-Vector3.forward * OpenDistance);
        _rightTargetPos = _rightDoorRigidbody.transform.TransformPoint(Vector3.forward * OpenDistance);
    }

    private void FixedUpdate()
    {
        if (_doorDetector.IsDetected)
        {

            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        if (_openCoroutine != null)
            return;
        
        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
            _closeCoroutine = null;
        }
        
        _openCoroutine = StartCoroutine(OpenDoorRoutine());
    }

    private void CloseDoor()
    {
        if (_closeCoroutine != null)
            return;

        if (_openCoroutine != null)
        {
            StopCoroutine(_openCoroutine);
            _openCoroutine = null;
        }
        
        _closeCoroutine = StartCoroutine(CloseDoorRoutine());
    }
    
    private IEnumerator OpenDoorRoutine()
    {
        while(Vector3.Distance(_leftDoorRigidbody.position, _leftTargetPos) > 0.01f || Vector3.Distance(_rightDoorRigidbody.position, _rightTargetPos) > 0.01f)
        {
            _leftDoorRigidbody.MovePosition(Vector3.MoveTowards(_leftDoorRigidbody.position, _leftTargetPos,
                OpenSpeed * Time.fixedDeltaTime));
            _rightDoorRigidbody.MovePosition(Vector3.MoveTowards(_rightDoorRigidbody.position, _rightTargetPos,
                OpenSpeed * Time.fixedDeltaTime));
            yield return null;
        }
    }

    private IEnumerator CloseDoorRoutine()
    {
        while (Vector3.Distance(_leftDoorRigidbody.position, _leftDoorStartPos) > 0.01f || Vector3.Distance(_rightDoorRigidbody.position, _rightDoorStartPos) > 0.01f)
        {
            _leftDoorRigidbody.MovePosition(Vector3.MoveTowards(_leftDoorRigidbody.position, _leftDoorStartPos,
                OpenSpeed * Time.fixedDeltaTime));
            _rightDoorRigidbody.MovePosition(Vector3.MoveTowards(_rightDoorRigidbody.position, _rightDoorStartPos,
                OpenSpeed * Time.fixedDeltaTime));
            yield return null;
        }
    }
}