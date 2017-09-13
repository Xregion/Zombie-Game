using UnityEngine;

public class EnemyAnimations : Animations {

    AIController aiController;

    void Start()
    {
        aiController = GetComponentInParent<AIController>();
        animator = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
    }

    public void SetIsEating(bool _isEating)
    {
        animator.SetBool("Eat", _isEating);
    }

    public void HitPlayer ()
    {
        aiController.Attack();
    }

    public override void MeleeingAnimationEnded()
    {
        base.MeleeingAnimationEnded();
        aiController.SetIsAttacking();
    }
}
