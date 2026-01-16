
using System;
using UnityEngine;
public class EnemyAttackHandler: MonoBehaviour
{
    [SerializeField] private EnemyAnimationHandler _enemyAnimationHandler;
    [SerializeField] private DamageSource _damageSource;

    public void OnEnable()
    {
        _enemyAnimationHandler.OnAttackHitFrame += SetDamageSourceActive;
        _enemyAnimationHandler.OnAttackEndFrame += SetDamageSourceActive;
    }
    
    public void OnDisable()
    {
        _enemyAnimationHandler.OnAttackHitFrame -= SetDamageSourceActive;
        _enemyAnimationHandler.OnAttackEndFrame -= SetDamageSourceActive;
    }

    public void Attack()
    {
        _enemyAnimationHandler.Attack();
    }
    
    private void SetDamageSourceActive(bool isAttacking)
    {
        _damageSource.SetActive(isAttacking);
    }
}
