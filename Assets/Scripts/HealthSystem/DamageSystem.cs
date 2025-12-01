using System;

public static class DamageSystem
{
    public static void ApplyDamage(IDamageSource source, IHealth target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (source == null)
             throw new ArgumentNullException(nameof(source));
        
        if (source.Owner != null && source.Owner == target)
            return;

        int damage = source.Damage;
        
        if (damage > 0)
        {
            target.TakeDamage(damage);
        }
    }
}