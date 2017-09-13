using UnityEngine;

public class Animations : MonoBehaviour {

    protected Animator animator;
    protected SpriteRenderer ren;

    public void SetIsMoving(bool _isMoving)
    {
        animator.SetBool("Moving", _isMoving);
    }

    public void SetIsMeleeing(bool _isMeleeing)
    {
        animator.SetBool("Melee", _isMeleeing);
    }

    public void SetIsDead()
    {
        animator.SetBool("Dead", false);
    }

    public void SetIsDead(bool _isDead)
    {
        animator.SetBool("Dead", _isDead);
    }

    public virtual void MeleeingAnimationEnded()
    {
        SetIsMeleeing(false);
    }
}
