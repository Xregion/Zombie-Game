using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public Text bulletsText;

    public LayerMask damageable;
    public Transform firePoint;
    public LineRenderer redDot;
    public int totalBulletsRemaining;
    public int maxBulletsInChamber;
    public float fireRate;
    public float meleeSpeed;
    public float reloadSpeed;
    public float bulletRange;
    public float meleeRange;
    public int bulletDamage;
    public int meleeDamage;
    public float critChance;
    [HideInInspector]
    public bool isReloading;
    [HideInInspector]
    public bool isFiring;
    [HideInInspector]
    public bool isMeleeing;
    [HideInInspector]
    public bool chamberIsEmpty;
    [HideInInspector]
    public bool fullChamber;
    [HideInInspector]
    public bool outOfBullets;

    int bulletsFired;
    int bulletsInChamber;

    void Start()
    {
        redDot.SetPosition(1, new Vector3(bulletRange, 0, 0));
        bulletsInChamber = maxBulletsInChamber;
        bulletsFired = bulletsInChamber;
        SetBulletsText();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, bulletRange, damageable);
        if (hit.collider != null)
            redDot.SetPosition(1, new Vector3(Vector3.Distance(hit.transform.position, firePoint.position), 0, 0));
        else
            redDot.SetPosition(1, new Vector3(bulletRange, 0, 0));

    }

    public void Fire ()
    {
        isFiring = true;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, bulletRange, damageable);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, meleeRange, damageable);
        if (hit.collider != null)
        {
            IDamageable hitObject = hit.transform.gameObject.GetComponent<IDamageable>();
            if (hitObject != null)
                hitObject.TakeDamage(meleeDamage);
        }
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

    public void SetRedDot (bool on)
    {
        redDot.enabled = on;
    }

    public void SetBulletsText ()
    {
        bulletsText.text = bulletsInChamber + "/" + totalBulletsRemaining;
    }
}
