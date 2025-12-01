using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Health : IHealth
{
    private readonly HealthData _healthData;
    private int _currentHealth;
    private bool _isAlive;
    
    public event Action OnChanged;
    public event Action OnTakeDamage;
    public event Action OnDeath;

    public Health(HealthData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        
        _healthData = data;
        _currentHealth = _healthData.MaxHealth;
        _isAlive = _currentHealth > 0; 
    }
    
    public void TakeDamage(int damage)
    {
        if (!_isAlive) return;
        
        OnTakeDamage?.Invoke();
        Change(damage); 
    }

    public void Heal(int amount)
    {
        if (!_isAlive || amount <= 0) return; 

        Change(-amount); 
    }

    protected void Change(int amount)
    {
        if (amount == 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        
        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _healthData.MaxHealth);

        if (_currentHealth <= 0 && _isAlive)
        {
            _currentHealth = 0;
            Die();
        }

        if (oldHealth != _currentHealth) // Только если значение реально изменилось
        {
            OnChanged?.Invoke();
        }
    }

    protected void Die()
    {
        _isAlive = false;
        OnDeath?.Invoke();
        HandleDeath();
    }
    
    protected abstract void HandleDeath();
}
