using System;
using UnityEngine;

public abstract class DamageSource : MonoBehaviour, IDamageSource
{
    private bool _isDamageHasDealt = false;
    [field: SerializeField] public int Damage { get; private set; }
    public IHealth Owner { get; private set; }

    private void Awake()
    {
        SetActive(false);
        Owner = GetComponentInParent<Runner>()?.GetComponent<IHealth>();
        Debug.Log(Owner);
    }

    public void SetActive(bool enabled)
    {
        this.enabled = enabled;
        Debug.Log($"ElectricBaton: {(enabled ? "включён" : "выключен")}");
        
        if (enabled == false)
        {
            _isDamageHasDealt = false;
        }
    }

    private  void OnTriggerEnter(Collider other)
    {
        if (_isDamageHasDealt)
            return; 
        
        if (other.TryGetComponent<IHealth>(out IHealth targetHealth))
        {
            Debug.Log(targetHealth);
            
            if (targetHealth != Owner)
            {
                Debug.Log("Триггер попал");
                DamageHandler.ApplyDamage(this, targetHealth);
                _isDamageHasDealt = true;
            }
        }
    }
}