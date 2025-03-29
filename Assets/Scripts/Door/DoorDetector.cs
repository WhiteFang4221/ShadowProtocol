using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    private readonly List<Collider> _triggeredColliders = new(); 
    
    public bool IsDetected { get; private set; }

    private void Update()
    {
        IsDetected = _triggeredColliders.Count > 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player playerMovement))
        {
            _triggeredColliders.Add(collider.gameObject.GetComponent<Collider>());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (_triggeredColliders.Contains(collider))
        {
            _triggeredColliders.Remove(collider);
        } 
    }
}
