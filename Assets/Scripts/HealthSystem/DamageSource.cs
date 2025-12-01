using System;
using UnityEngine;

public class DamageSource : MonoBehaviour, IDamageSource
{
    [field: SerializeField] public int Damage { get; private set; }
    public IHealth Owner { get; private set; }

    private void Awake()
    {
        Owner = GetComponentInParent<IHealth>();
    }

    public void ProcessDamage(IHealth target)
    {
        DamageSystem.ApplyDamage(this, target);
    }
}