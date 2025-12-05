using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;

    public event Action OnStartAttack;

    public virtual void Attack()
    {
        OnStartAttack?.Invoke();
    }
}
