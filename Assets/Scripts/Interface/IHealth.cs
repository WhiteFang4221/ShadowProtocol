using System;

public interface IHealth
{
    public event Action OnChanged;
    public event Action OnTakeDamage;
    event Action OnDeath;
    
    public void TakeDamage(int damage);
    
}