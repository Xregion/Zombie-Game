using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    Animator animator;
    bool isMoving;
    bool isFiring;
    bool isReloading;
    bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Fire", isFiring);
        animator.SetBool("Reload", isReloading);
        animator.SetBool("Dead", isDead);
    }

    public void SetIsMoving(bool _isMoving)
    {
        isMoving = _isMoving;
    }

    public void SetIsFiring(bool _isFiring)
    {
        isFiring = _isFiring;
    }

    public void SetIsReloading(bool _isReloading)
    {
        isReloading = _isReloading;
    }

    public void SetIsDead(bool _isDead)
    {
        isDead = _isDead;
    }
}
