using System.Collections;
using UnityEngine;

public class SlidingDoorMover
{
    private const float OpenSpeed = 8f; 
    private const float DistanceThreshold = 0.01f;

    private readonly Rigidbody _leftDoorRigidbody;
    private readonly Rigidbody _rightDoorRigidbody;
    private readonly Vector3 _leftStartPos;
    private readonly Vector3 _rightStartPos;
    private readonly Vector3 _leftTargetPos;
    private readonly Vector3 _rightTargetPos;

    private Coroutine _currentMoveCoroutine;

    public SlidingDoorMover(Rigidbody leftDoorRb, Rigidbody rightDoorRb, float openDistance)
    {
        _leftDoorRigidbody = leftDoorRb;
        _rightDoorRigidbody = rightDoorRb;

        _leftStartPos = leftDoorRb.position;
        _rightStartPos = rightDoorRb.position;

        _leftTargetPos = leftDoorRb.transform.TransformPoint(-Vector3.forward * openDistance);
        _rightTargetPos = rightDoorRb.transform.TransformPoint(Vector3.forward * openDistance);
    }

    public void OpenDoor(MonoBehaviour coroutineRunner, System.Action onMoveComplete = null)
    {
        StartMove(coroutineRunner, _leftTargetPos, _rightTargetPos, onMoveComplete);
    }

    public void CloseDoor(MonoBehaviour coroutineRunner, System.Action onMoveComplete = null)
    {
        StartMove(coroutineRunner, _leftStartPos, _rightStartPos, onMoveComplete);
    }

    private void StartMove(MonoBehaviour coroutineRunner, Vector3 leftTarget, Vector3 rightTarget, System.Action onMoveComplete)
    {
        if (_currentMoveCoroutine != null)
        {
            coroutineRunner.StopCoroutine(_currentMoveCoroutine);
        }

        _currentMoveCoroutine = coroutineRunner.StartCoroutine(MoveRoutine(leftTarget, rightTarget, () => {
            _currentMoveCoroutine = null;
            onMoveComplete?.Invoke();
        }));
    }

    private IEnumerator MoveRoutine(Vector3 leftTarget, Vector3 rightTarget, System.Action onComplete)
    {
        while (Vector3.Distance(_leftDoorRigidbody.position, leftTarget) > DistanceThreshold ||
               Vector3.Distance(_rightDoorRigidbody.position, rightTarget) > DistanceThreshold)
        {
            _leftDoorRigidbody.MovePosition(Vector3.MoveTowards(_leftDoorRigidbody.position, leftTarget, OpenSpeed * Time.fixedDeltaTime));
            _rightDoorRigidbody.MovePosition(Vector3.MoveTowards(_rightDoorRigidbody.position, rightTarget, OpenSpeed * Time.fixedDeltaTime));
            yield return null;
        }
        onComplete?.Invoke();
    }
}