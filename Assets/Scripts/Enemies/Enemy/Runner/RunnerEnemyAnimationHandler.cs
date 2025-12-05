using System;

public class RunnerEnemyAnimationHandler : EnemyAnimationHandler
{
    public const string AttackTrigger = "AttackTrigger";

    public event Action OnStartAttack;
    
    public override void Attack()
    {
        Animator.SetTrigger(AttackTrigger);
        OnStartAttack?.Invoke();
    }
}
