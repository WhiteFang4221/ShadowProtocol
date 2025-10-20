using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    private List<IDoorEnterable> _triggeredObjects = new();
    public IReadOnlyList<IDoorEnterable> TriggeredObjects => _triggeredObjects;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDoorEnterable doorAccessible))
        {
            Debug.Log("Entered door: " + doorAccessible);
            _triggeredObjects.Add(doorAccessible);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDoorEnterable doorAccessible))
        {
            _triggeredObjects.Remove(doorAccessible);
        }
    }
}
