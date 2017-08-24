using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(GunController))]
public class PlayerController : MonoBehaviour {

    public event Action<float> HealthChangeEvent;
    public event Action DeathEvent;


    public SpriteRenderer muzzleFlash;
    public float normalMovSpeed;
    public float runMovSpeed;
    public float movSpeedWhileReloading;
    public float backpeddleMovSpeed;
    [HideInInspector]
    public int currentHealth;
    public int totalHealth;

    float horizontalDirection;
    float verticalDirection;
    float mouseToPlayerAngle;
    float movSpeed;
    Vector3 mousePosition;
    Vector3 playerRotation;
    bool controlsOn;
    bool isRunning;
    GunController gunController;
    PlayerAnimation animations;

    void Start()
    {
        currentHealth = totalHealth;
        movSpeed = normalMovSpeed;
        playerRotation = new Vector3(0, 0, 0);
        gunController = GetComponent<GunController>();
        animations = GetComponentInChildren<PlayerAnimation>();
        controlsOn = true;
        muzzleFlash.gameObject.SetActive(false);
    }
	
	void Update()
    {
        if (controlsOn)
        {
            if (Input.GetButtonDown("Fire1") && !IsPerformingAction())
            {
                gunController.Fire();
                if (!gunController.chamberIsEmpty)
                {
                    muzzleFlash.gameObject.SetActive(true);
                    animations.SetIsFiring(true);
                    StartCoroutine(AnimationTimer(gunController.fireRate));
                }
            }
            else if (Input.GetButtonDown("Reload") && !IsPerformingAction())
            {
                gunController.Reload();
                if (!gunController.fullChamber && !gunController.outOfBullets)
                {
                    movSpeed = movSpeedWhileReloading;
                    animations.SetIsReloading(true);
                    StartCoroutine(AnimationTimer(gunController.reloadSpeed));
                }
            }
            else if (Input.GetButton("Run") && !IsPerformingAction())
            {
                isRunning = true;
                movSpeed = runMovSpeed;
            }

            if (Input.GetButtonUp("Run"))
                isRunning = false;
        }
        else
        {
            if (DeathEvent != null)
            {
                DeathEvent();
            }
        }
    }

    void FixedUpdate()
    {
        if (controlsOn)
        {
            // Have player look at cursor at all times
            mousePosition = Input.mousePosition;
            mousePosition.z = transform.position.z;
            Vector3 worldPointMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (worldPointMousePosition.y > transform.position.y)
                mouseToPlayerAngle = Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + 
                    Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
            else
                mouseToPlayerAngle = -Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + 
                    Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
            playerRotation.z = mouseToPlayerAngle;
            transform.eulerAngles = (playerRotation);

            horizontalDirection = Input.GetAxis("Horizontal");
            verticalDirection = Input.GetAxis("Vertical");
            if (horizontalDirection != 0)
            {
                Strafe();
            }
            if (verticalDirection != 0)
            {
                if (verticalDirection < 0)
                    movSpeed = backpeddleMovSpeed;
                else if (!Input.GetButton("Run"))
                    movSpeed = normalMovSpeed;

                MoveCharacter();
            }
            if (horizontalDirection == 0 && verticalDirection == 0)
            {
                SetMovementAnimation(false);
            }
        }
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;
        if (HealthChangeEvent != null)
            HealthChangeEvent(-damage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Health"))
        {
            HealthPack healthPack = collision.GetComponent<HealthPack>();
            print(healthPack.GetAmountOfHealth());
            currentHealth += healthPack.GetAmountOfHealth();
            healthPack.Destroy();

            if (HealthChangeEvent != null)
                HealthChangeEvent(healthPack.GetAmountOfHealth());
        }
        else if (collision.CompareTag("Ammo"))
        {
            AmmoPack ammoPack = collision.GetComponent<AmmoPack>();
            gunController.totalBulletsRemaining += ammoPack.GetAmountOfBullets();
            gunController.SetBulletsText();
            ammoPack.Destroy();
        }
    }

    bool IsPerformingAction ()
    {
        return gunController.isFiring || gunController.isReloading || isRunning;
    }

    void MoveCharacter()
    {
        transform.Translate(new Vector3(verticalDirection * movSpeed * Time.deltaTime, 0, 0));
        SetMovementAnimation(true);
    }

    void Strafe()
    {
        transform.Translate(new Vector3(0, -horizontalDirection * movSpeed * Time.deltaTime, 0));
        SetMovementAnimation(true);
    }

    void SetMovementAnimation(bool isMoving)
    {
        animations.SetIsMoving(isMoving);
    }

    void Die()
    {
        controlsOn = false;
        SetMovementAnimation(false);
        animations.SetIsDead(true);
    }

    IEnumerator AnimationTimer (float timer)
    {
        yield return new WaitForSeconds(timer);

        muzzleFlash.gameObject.SetActive(false);
        gunController.isFiring = false;
        gunController.isReloading = false;
        animations.SetIsFiring(false);
        animations.SetIsReloading(false);
        movSpeed = normalMovSpeed;
    }
}
