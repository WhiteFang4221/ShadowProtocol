using System;

class DamageSystem: IDamageSource
{
    public void ApplyDamage(IHealth target, int damage)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (damage <= 0)
            throw new ArgumentOutOfRangeException(nameof(damage));
        
        target.TakeDamage(damage);
    }
}