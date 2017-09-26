using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public Text bulletsText;

    public LayerMask damageable;
    public Transform firePoint;
    public LineRenderer redDot;
    public SpriteRenderer muzzleFlash;
    public int totalBulletsRemaining;
    public int maxBulletsInChamber;
    public float bulletRange;
    public int bulletDamage;
    public int meleeDamage;
    public float critChance;

    bool isReloading;
    bool isFiring;
    bool isMeleeing;
    bool chamberIsEmpty;
    bool fullChamber;
    bool outOfBullets;
    bool meleeHit;
    BoxCollider2D gunCollider;
    RaycastHit2D hit;
    int bulletsFired;
    int bulletsInChamber;

    void Start()
    {
        gunCollider = GetComponent<BoxCollider2D>();
        gunCollider.enabled = false;
        muzzleFlash.gameObject.SetActive(false);
        redDot.SetPosition(1, new Vector3(bulletRange, 0, 0));
        bulletsInChamber = maxBulletsInChamber;
        bulletsFired = bulletsInChamber;
        SetBulletsText();
    }

    void Update()
    {
        hit = Physics2D.Raycast(firePoint.position, firePoint.right, bulletRange, damageable);
        if (hit.collider != null && !hit.collider.isTrigger)
            redDot.SetPosition(1, new Vector3(Vector3.Distance(hit.point, firePoint.position), 0, 0));
        else
            redDot.SetPosition(1, new Vector3(bulletRange, 0, 0));

    }

    public void Fire ()
    {
        muzzleFlash.gameObject.SetActive(true);
        isFiring = true;
        if (hit.collider != null)
        {
            IDamageable hitObject = hit.transform.gameObject.GetComponent<IDamageable>();
            if (hitObject != null)
            {
                float crit = Random.Range(0f, 1f);
                if (crit <= critChance)
                    hitObject.TakeDamage(bulletDamage * 2);
                else
                    hitObject.TakeDamage(bulletDamage);
            }
        }
        bulletsInChamber--;
        if (bulletsInChamber <= 0)
            chamberIsEmpty = true;

        SetBulletsText();
    }

    public void MeleeAttack ()
    {
        isMeleeing = true;
        gunCollider.enabled = true;
    }

    public void Reload ()
    {
        if (bulletsInChamber == maxBulletsInChamber)
        {
            fullChamber = true;
            return;
        }

        if (totalBulletsRemaining > 0)
        {
            fullChamber = false;
            isReloading = true;
            chamberIsEmpty = false;
            bulletsFired = maxBulletsInChamber - bulletsInChamber;
            if (totalBulletsRemaining >= maxBulletsInChamber || (bulletsInChamber > 0 && totalBulletsRemaining < maxBulletsInChamber))
                bulletsInChamber = maxBulletsInChamber;
            else
                bulletsInChamber = totalBulletsRemaining;

            totalBulletsRemaining -= bulletsFired;
            if (totalBulletsRemaining < 0)
                totalBulletsRemaining = 0;
        }
        else
        {
            // Play out of ammo audio clip
            outOfBullets = true;
        }
        SetBulletsText();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hitObject = collision.GetComponent<IDamageable>();
        if (hitObject != null && !meleeHit)
        {
            hitObject.TakeDamage(meleeDamage);
            meleeHit = true;
        }
    }

    public bool getIsReloading()
    {
        return isReloading;
    }

    public void SetIsReloading(bool _isReloading)
    {
        isReloading = _isReloading;
    }

    public bool getIsFiring()
    {
        return isFiring;
    }

    public void SetIsFiring(bool _isFiring)
    {
        isFiring = _isFiring;
    }

    public bool getIsMeleeing()
    {
        return isMeleeing;
    }

    public void SetIsMeleeing(bool _isMeleeing)
    {
        isMeleeing = _isMeleeing;
    }

    public bool getChamberIsEmpty()
    {
        return chamberIsEmpty;
    }

    public bool getFullChamber()
    {
        return fullChamber;
    }

    public bool getOutOfBullets()
    {
        return outOfBullets;
    }

    public void SetMeleeHit(bool _meleeHit)
    {
        meleeHit = _meleeHit;
    }

    public void SetRedDot (bool on)
    {
        redDot.enabled = on;
    }

    public void SetBulletsText ()
    {
        bulletsText.text = bulletsInChamber + "/" + totalBulletsRemaining;
    }

    public void DisableGunCollider ()
    {
        gunCollider.enabled = false;
    }
}
