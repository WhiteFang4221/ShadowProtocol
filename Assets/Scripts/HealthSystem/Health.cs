using System;
using UnityEngine;

public abstract class Health
{
    private readonly HealthConfig _healthConfig;
    private int _currentHealth;
    private bool _isAlive;
    
    public int CurrentHealth => _currentHealth;
    public event Action OnChanged;
    public event Action OnTakeDamage;
    public event Action OnDeath;

    public Health(HealthConfig config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));
        
        _healthConfig = config;
        _currentHealth = _healthConfig.MaxHealth;
        _isAlive = _currentHealth > 0; 
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("Наносят урон " + damage);
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
        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _healthConfig.MaxHealth);

        if (_currentHealth <= 0 && _isAlive)
        {
            _currentHealth = 0;
            Die();
        }

        if (oldHealth != _currentHealth) 
        {
            OnChanged?.Invoke();
        }
        
        Debug.Log ("Здоровье объекта: " + _currentHealth);
    }

    protected void Die()
    {
        _isAlive = false;
        OnDeath?.Invoke();
        HandleDeath();
    }
    
    protected abstract void HandleDeath();
}
