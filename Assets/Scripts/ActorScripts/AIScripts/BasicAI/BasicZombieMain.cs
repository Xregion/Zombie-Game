using UnityEngine;
using System;

public class BasicZombieMain : AIController {

    public event Action Died;

    public float aggroRange;

    bool playerEnteredView;
    GameObject lastKnownPosition;

    void Start()
    {
        lastKnownPosition = new GameObject("Player's Last Known Position");
    }

    protected override void Movement()
    {
        base.Movement();

        // if the player is in view look at him
        if (CheckIfPlayerIsInView() && distanceToPlayer <= aggroRange)
        {
            if (!playerEnteredView)
            {
                StartCoroutine(PlayAudioClip(playerSeen, false));
                target = player;
                playerEnteredView = true;
            }
            motor.SetTarget(player.transform.position);
            SaveManager.data.PlayerIsInCombat = true;
        }
        else if (playerEnteredView)
        {
            SaveManager.data.PlayerIsInCombat = false;
            lastKnownPosition.transform.position = player.transform.position;
            motor.SetTarget(lastKnownPosition.transform.position);
            target = lastKnownPosition;
            playerEnteredView = false;
            StartCoroutine(PlayAudioClip(idlingClip, false));
        }

        // check if the Actor is allowed to move and if so use the motor to move him towards his target at specified speed
        // otherwise attack the current target if it is in range
        if (!CheckIfInRange() && !isStaggered && !isAttacking && (playerEnteredView || target == lastKnownPosition))
        {
            if ((target == player && distanceToPlayer <= aggroRange) || (target == lastKnownPosition && Vector3.Distance(transform.position, target.transform.position) >= 2f))
            {
                if (playerIsDead)
                {
                    if (!isBlocked)
                    {
                        motor.MoveActor(new Vector3(1, 0, 0), movSpeed);
                        animations.SetIsMoving(true);
                    }
                }
                else
                {
                    motor.MoveActor(new Vector3(1, 0, 0), movSpeed);
                    animations.SetIsMoving(true);
                }
            }
            else
                animations.SetIsMoving(false);
        }
        else if (CheckIfInRange() && !playerIsDead)
        {
            animations.SetIsMeleeing(true);
            isAttacking = true;
        }
    }

    protected override void Die()
    {
        base.Die();
        SaveManager.data.PlayerIsInCombat = false;
        if (Died != null)
            Died();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CheckIfPlayerIsInView()) // if the player isn't in view then check if the object collided with is able to be damaged and is not a fellow enemy actor
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null && !collision.gameObject.CompareTag("Enemy"))
            {
                if (!target.CompareTag("Environment"))
                {
                    target = collision.gameObject; // if it is able to be damaged then set it as the target and set in range to be true
                    motor.SetTarget(target.transform.position);
                }
            }
        }
    }
}
