using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    private List<IDoorEnterable> _triggeredObjects = new(); 
    
    public IReadOnlyList<IDoorEnterable> TriggeredObjects => _triggeredObjects;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDoorEnterable doorAccessable))
        {
            Debug.Log("Entered door: " + doorAccessable);
            _triggeredObjects.Add(doorAccessable);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDoorEnterable doorAccessable))
        {
            _triggeredObjects.Remove(doorAccessable);
        }
    }
}
