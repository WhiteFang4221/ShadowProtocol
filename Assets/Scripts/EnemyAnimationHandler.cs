using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string AttackTrigger = "AttackTrigger";
    public event Action<bool> OnAttackHitFrame;
    public event Action<bool> OnAttackEndFrame;

    public void Attack()
    {
        _animator.SetTrigger(AttackTrigger);
    }
    
    public void NotifyAttackHitFrame()
    {
        OnAttackHitFrame?.Invoke(true);
    }

    public void NotifyAttackEndFrame()
    {
        OnAttackEndFrame?.Invoke(false);
    }
}
