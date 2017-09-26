using UnityEngine;

public class PlayerAnimation : Animations {

    GunController gunController;
    PlayerController player;

    void Start()
    {
        gunController = transform.parent.GetComponentInChildren<GunController>();
        player = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
    }

    public void SetIsFiring(bool _isFiring)
    {
        animator.SetBool("Fire", _isFiring);
    }

    public void SetIsReloading(bool _isReloading)
    {
        animator.SetBool("Reload", _isReloading);
    }

    public void ShootingAnimationEnded()
    {
        gunController.SetIsFiring(false);
        SetIsFiring(false);
        gunController.muzzleFlash.gameObject.SetActive(false);
    }

    public override void MeleeingAnimationEnded ()
    {
        base.MeleeingAnimationEnded();
        gunController.SetIsMeleeing(false);
        SetIsMeleeing(false);
        gunController.DisableGunCollider();
        gunController.SetRedDot(true);
        gunController.SetMeleeHit(false);
        ResetMoveSpeed();
    }

    public void ReloadingAnimationEnded ()
    {
        gunController.SetIsReloading(false);
        SetIsReloading(false);
        gunController.SetRedDot(true);
        ResetMoveSpeed();
    }

    public void ResetMoveSpeed ()
    {
        player.ResetMoveSpeed();
    }
}
