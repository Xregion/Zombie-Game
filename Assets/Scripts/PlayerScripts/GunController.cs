using UnityEngine;

public class GunController : MonoBehaviour {

    public LayerMask shootable;
    public Transform firePoint;
    public float totalBulletsRemaining;
    public float maxBulletsInChamber;
    public float fireRate;
    public float range;
    public float damage;
    [HideInInspector]
    public bool reloading;

    float bulletsRemainingInChamber;
    float bulletsInChamber;

    void Start()
    {
        bulletsInChamber = maxBulletsInChamber;
        bulletsRemainingInChamber = bulletsInChamber;
    }

    public void Fire ()
    {
        if (bulletsInChamber <= 0)
        {
            Reload();
        }
        else if (!reloading)
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range, shootable);
            if (hit.collider != null)
            {
                PlayerController hitPlayer = hit.transform.gameObject.GetComponent<PlayerController>();
                if (hitPlayer != null)
                {
                    hitPlayer.TakeDamage(damage);
                }
            }
            bulletsInChamber--;
        }
    }

    public void Reload ()
    {
        if (bulletsInChamber == maxBulletsInChamber)
        {
            return;
        }
        reloading = true;
        // start reloading animation
        if (totalBulletsRemaining > 0)
        {
            bulletsRemainingInChamber = maxBulletsInChamber - bulletsInChamber;
            if (totalBulletsRemaining > maxBulletsInChamber)
            {
                bulletsInChamber = maxBulletsInChamber;
            }
            else
            {
                bulletsInChamber = totalBulletsRemaining;
            }
            totalBulletsRemaining -= bulletsRemainingInChamber;
            if (totalBulletsRemaining < 0)
            {
                totalBulletsRemaining = 0;
            }
        }
        else
        {
            // Play out of ammo audio clip
        }

        reloading = false;
    }
}
