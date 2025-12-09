using System;

public static class DamageHandler
{
    public static void ApplyDamage(IDamageSource source, IHealth target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (source == null)
             throw new ArgumentNullException(nameof(source));

        int damage = source.Damage;
        
        if (damage > 0)
        {
            target.Health.TakeDamage(damage);
        }
    }
}