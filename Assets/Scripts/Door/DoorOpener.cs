using System.Collections;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private const float OpenDistance = 1f;

    [SerializeField] private KeyCard _requiredKeyCard;
    [SerializeField] private DoorDetector _doorDetector;
    [SerializeField] private Rigidbody _leftDoorRigidbody;
    [SerializeField] private Rigidbody _rightDoorRigidbody;

    private SlidingDoorMover _doorMover;
    private bool _isDoorOpen = false;

    private void Awake()
    {
        _doorMover = new SlidingDoorMover(_leftDoorRigidbody, _rightDoorRigidbody, OpenDistance);
    }

    private void FixedUpdate()
    {
        EvaluateDoorState();
    }

    private void EvaluateDoorState()
    {
        bool isShouldOpen = false;

        if (_doorDetector.TriggeredObjects.Count > 0)
        {
            if (_requiredKeyCard == KeyCard.None)
            {
                isShouldOpen = true;
            }
            else
            {
                isShouldOpen = IsHasKeyCard();
            }
        }

        if (isShouldOpen && !_isDoorOpen)
        {
            OpenDoor();
        }
        else if (!isShouldOpen && _isDoorOpen)
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        if (!_isDoorOpen)
            _doorMover.OpenDoor(this, () => _isDoorOpen = true);
    }

    private void CloseDoor()
    {
        if (_isDoorOpen)
            _doorMover.CloseDoor(this, () => _isDoorOpen = false);
    }

    private bool IsHasKeyCard()
    {
        foreach (IDoorEnterable enterable in _doorDetector.TriggeredObjects)
        {
            if (enterable.KeyCards.Contains(_requiredKeyCard))
            {
                return true;
            }
        }
        return false;
    }
}