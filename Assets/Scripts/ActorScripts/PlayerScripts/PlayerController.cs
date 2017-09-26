﻿using UnityEngine;
using System;

[RequireComponent(typeof(ActorMotor))]
public class PlayerController : MonoBehaviour, IDamageable {

    public event Action<float> HealthChangeEvent; // called when the health of the player either increases or decreases
    public event Action DeathEvent; // called when the player dies


    public float normalMoveSpeed; // the normal movement speed set in the inspector
    public float runMoveSpeed; // the movement speed when the player is running
    public float moveSpeedWhileReloading; // the movement speed when the player is reloading
    public float backpeddleMoveSpeed; // the movement speed when the player is walking backwards

    [HideInInspector]
    public int currentHealth;
    public int totalHealth;

    //float horizontalDirection;
    float verticalDirection;
    float mouseToPlayerAngle;
    float movSpeed;
    Vector3 mousePosition;
    Vector3 worldPointMousePosition;
    bool controlsOn;
    bool isRunning;
    bool isMoving;
    GunController gunController;
    PlayerAnimation animations;
    ActorMotor motor;

    void Start()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z;
        Vector3 worldPointMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        currentHealth = totalHealth;
        movSpeed = normalMoveSpeed;
        gunController = GetComponentInChildren<GunController>();
        animations = GetComponentInChildren<PlayerAnimation>();
        motor = GetComponent<ActorMotor>();
        motor.SetTarget(worldPointMousePosition);
        controlsOn = true;
    }
	
	void Update()
    {
        if (controlsOn)
        {
            if (Input.GetButtonDown("Fire1") && !IsPerformingAction()) // check if the player hit the fire button and is not currently performing another action
            {
                if (!gunController.getChamberIsEmpty())
                {
                    gunController.Fire(); // calls the Fire method from the gun controller
                    animations.SetIsFiring(true);
                }
                else
                {
                    gunController.Reload();
                    if (!gunController.getOutOfBullets())
                    {
                        movSpeed = moveSpeedWhileReloading;
                        animations.SetIsReloading(true);
                    }
                }
            }
            else if (Input.GetButtonDown("Reload") && !IsPerformingAction())
            {
                gunController.Reload();
                if (!gunController.getFullChamber() && !gunController.getOutOfBullets())
                {
                    movSpeed = moveSpeedWhileReloading;
                    animations.SetIsReloading(true);
                    gunController.SetRedDot(false);
                }
            }
            else if (Input.GetButtonDown("Melee") && !IsPerformingAction() && !isMoving) {
                gunController.MeleeAttack();
                animations.SetIsMeleeing(true);
                movSpeed = 0;
                gunController.SetRedDot(false);
            }
            else if (Input.GetButton("Run") && !IsPerformingAction())
            {
                isRunning = true;
                movSpeed = runMoveSpeed;
            }

            if (Input.GetButtonUp("Run") && !gunController.getIsReloading())
            {
                isRunning = false;
                movSpeed = normalMoveSpeed;
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
            motor.SetTarget(worldPointMousePosition);

            //horizontalDirection = Input.GetAxisRaw("Horizontal");
            verticalDirection = Input.GetAxisRaw("Vertical");

            if (verticalDirection < 0)
                movSpeed = backpeddleMoveSpeed;
            else if (!gunController.getIsReloading() && !isRunning)
                movSpeed = normalMoveSpeed;

            motor.MoveActor(new Vector3 (verticalDirection, 0, 0), movSpeed);

            SetMovementAnimation(true);
            isMoving = true;

            if (verticalDirection == 0)
            {
                SetMovementAnimation(false);
                isMoving = false;
            }
        }
    }

    public bool TakeDamage (int damage)
    {
        currentHealth -= damage;
        if (HealthChangeEvent != null)
            HealthChangeEvent(-damage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return true;
        }
        return false;
    }

    public bool GetControls ()
    {
        return controlsOn;
    }

    public void SetControls (bool on)
    {
        controlsOn = on;
    }

    public void ResetMoveSpeed ()
    {
        movSpeed = normalMoveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Health"))
        {
            Item healthPack = collision.GetComponent<Item>();
            currentHealth += healthPack.GetAmountToDrop();
            if (currentHealth > totalHealth)
                currentHealth = totalHealth;

            if (HealthChangeEvent != null)
                HealthChangeEvent(healthPack.GetAmountToDrop());

            healthPack.Destroy();
        }
        else if (collision.CompareTag("Ammo"))
        {
            Item ammoPack = collision.GetComponent<Item>();
            gunController.totalBulletsRemaining += ammoPack.GetAmountToDrop();
            gunController.SetBulletsText();
            ammoPack.Destroy();
        }
    }

    // Checks if the player is shooting, reloading, or running. Used to block certain actions while doing any of these things
    bool IsPerformingAction ()
    {
        return gunController.getIsFiring() || gunController.getIsReloading() || isRunning || gunController.getIsMeleeing();
    }

    void SetMovementAnimation(bool isMoving)
    {
        animations.SetIsMoving(isMoving);
    }

    void Die()
    {
        if (DeathEvent != null)
            DeathEvent();

        controlsOn = false;
        SetMovementAnimation(false);
        animations.SetIsDead(true);
    }
}
