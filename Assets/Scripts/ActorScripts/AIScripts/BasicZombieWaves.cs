using UnityEngine;
using System;
using System.Collections;

public class BasicZombieWaves : AIController {

    public event Action<GameObject> DeathEvent;

    void Start()
    {
        target = player;
    }

    protected override void Movement()
    {
        base.Movement();

        // set the motors target to look at
        motor.SetTarget(target.transform.position);

        // check if the Actor is allowed to move and if so use the motor to move him towards his target at specified speed
        // otherwise attack the current target if it is in range
        if (!CheckIfInRange() && !isStaggered && !isAttacking)
        {
            motor.MoveActor(new Vector3(1, 0, 0), movSpeed);
            animations.SetIsMoving(true);
        }
        else if (CheckIfInRange())
        {
            animations.SetIsMeleeing(true);
            isAttacking = true;
        }
    }

    protected override void Die()
    {
        base.Die();
        if (DeathEvent != null)
            DeathEvent(gameObject);

        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        CancelInvoke();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckIfPlayerIsInView()) // if the player isn't in view then check if the object collided with is able to be damaged and is not a fellow enemy actor
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null && !collision.gameObject.CompareTag("Enemy"))
            {
                if (!target.CompareTag("Environment"))
                {
                    target = collision.gameObject; // if it is able to be damaged then set it as the target and set in range to be true
                }
            }
        }
    }
}
