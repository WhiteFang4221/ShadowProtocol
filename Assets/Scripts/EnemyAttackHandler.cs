
using System;
using UnityEngine;
public class EnemyAttackHandler: MonoBehaviour
{
    private EnemyAnimationHandler _enemyAnimationHandler;
    private DamageSource _damageSource;

    public void OnEnable()
    {
        _enemyAnimationHandler.OnStartAttack += OnAttackStarted;
    }

    public void Initialize(EnemyAnimationHandler enemyAnimationHandler, DamageSource damageSource)
    {
        _enemyAnimationHandler = enemyAnimationHandler;
        _damageSource = damageSource;
    }

    public void OnAttackStarted()
    {
        _damageSource.SetEnabled(true);
    }

    public void OnAttackFinished()
    {
        _damageSource.SetEnabled(false);
    }
}
