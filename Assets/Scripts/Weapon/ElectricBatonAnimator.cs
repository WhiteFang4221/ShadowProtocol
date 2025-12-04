using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ElectricBatonAnimator: MonoBehaviour, IWeaponAnimator
{
   [SerializeField] private Animator _animator;
   
   public void AttackAnimation()
   {
      _animator.SetTrigger("Attack");
   }
}