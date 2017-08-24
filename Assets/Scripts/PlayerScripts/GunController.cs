using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public Text bulletsText;

    public LayerMask shootable;
    public Transform firePoint;
    public LineRenderer redDot;
    public int totalBulletsRemaining;
    public int maxBulletsInChamber;
    public float fireRate;
    public float reloadSpeed;
    public float range;
    public float damage;
    public float critChance;
    [HideInInspector]
    public bool isReloading;
    [HideInInspector]
    public bool isFiring;
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
        redDot.SetPosition(1, new Vector3(range, 0, 0));
        bulletsInChamber = maxBulletsInChamber;
        bulletsFired = bulletsInChamber;
        SetBulletsText();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range, shootable);
        if (hit.collider != null)
            redDot.SetPosition(1, new Vector3(Vector3.Distance(hit.transform.position, transform.position), 0, 0));
        else
            redDot.SetPosition(1, new Vector3(range, 0, 0));

    }

    public void Fire ()
    {
        if (bulletsInChamber <= 0)
        {
            //Reload();
            chamberIsEmpty = true;
        }
        else if (!isReloading)
        {
            isFiring = true;
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range, shootable);
            if (hit.collider != null)
            {
                AIController zombieHit = hit.transform.gameObject.GetComponent<AIController>();
                if (zombieHit != null)
                {
                    float crit = Random.Range(0f, 1f);
                    if (crit <= critChance)
                        zombieHit.TakeDamage(damage * 2f);
                    else
                        zombieHit.TakeDamage(damage);
                }
            }
            bulletsInChamber--;
            SetBulletsText();
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

    public void SetBulletsText ()
    {
        bulletsText.text = bulletsInChamber + "/" + totalBulletsRemaining;
    }
}
