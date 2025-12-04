using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ElectricBaton : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private DamageSource _damageSource;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        DisableCollider();
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out IHealth health))
        {
            DamageHandler.ApplyDamage(_damageSource, health);
        }
    }
}
