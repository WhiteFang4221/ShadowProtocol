using System;
using UnityEngine;

public abstract class DamageSource : MonoBehaviour, IDamageSource
{
    [field: SerializeField] public int Damage { get; private set; }
    public IHealth Owner { get; private set; }

    private void Awake()
    {
        SetEnabled(false);
        Owner = GetComponentInParent<Runner>()?.GetComponent<IHealth>();
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
        Debug.Log($"ElectricBaton: {(enabled ? "включён" : "выключен")}");
    }
    
    public void ProcessDamage(IHealth target)
    {
        DamageHandler.ApplyDamage(this, target);
    }

    private  void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out Health health))
        {
            if (health != Owner)
            {
                DamageHandler.ApplyDamage(this, health);
            }
        }
    }
}