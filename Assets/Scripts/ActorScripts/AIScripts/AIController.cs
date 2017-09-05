using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ActorMotor))]
[RequireComponent(typeof(ItemDrop))]
public class AIController : MonoBehaviour, IDamageable {

    public event Action<GameObject> DeathEvent; // Called when zombie dies

    public LayerMask colliderMask;
    public float maxHealth;
    public float movSpeed;
    public int attackDamage;
    public float attackSpeed;
    public float staggerTime; // the amount of time after being shot that the AI can't move
    public float knockbackAmount; // the amount at x the AI will be knocked back when shot

    [HideInInspector]
    public bool isAlive;

    GameObject target;
    GameObject player;
    SpriteRenderer[] bloodSplatter;
    float currentHealth;
    IDamageable targetToDamage;
    ItemDrop dropper;
    ActorMotor motor;
    BoxCollider2D col;
    bool isAttacking;
    bool isStaggered;
    bool isInRange;
    bool playerIsInView;
    bool isBlocked;
    static bool playerIsDead;

    void OnEnable()
    {
        isAlive = true;
    }

    void Start () {
        col = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        player = GameObject.Find("Player"); // find the player in the world
        target = player; // set the target as the player
        bloodSplatter = GetComponentsInChildren<SpriteRenderer>(true);
        targetToDamage = target.GetComponent<IDamageable>();
        dropper = GetComponent<ItemDrop>();
        motor = GetComponent<ActorMotor>();
        motor.SetTarget(target.transform.position);
        bloodSplatter[1].enabled = false;

    }

    void Update () {
        if (isAlive)
        {
            CheckIfPlayerIsInView();

            // if the player is in view then target him
            if ((playerIsInView && target != player) || target == null)
            {
                target = player;
                isInRange = false;
            }


            // set the motors target to look at
            motor.SetTarget(target.transform.position);

            // check if the Actor is allowed to move and if so use the motor to move him towards his target at specified speed
            // otherwise attack the current target if it is in range
            if (IsAbleToMove())
                motor.MoveActor(1, movSpeed);
            else if (isInRange)
                Attack();

            if (playerIsDead && (isInRange || isBlocked))
            {
                motor.MoveActor(0, 0);
                // Start player eating animation
            }
        }
    }

    public void ReviveZombie()
    {
        currentHealth = maxHealth;
        isAlive = true;
        col.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!playerIsInView) // if the player isn't in view then check if the object collided with is able to be damaged and is not a fellow enemy actor
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null && !collision.gameObject.CompareTag("Enemy"))
            {
                if (!target.CompareTag("Environment"))
                {
                    target = collision.gameObject; // if it is able to be damage then set it as the target and set in range to be true
                    isInRange = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
            isInRange = true;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isBlocked = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isInRange = false;
    }

    void CheckIfPlayerIsInView ()
    {
        // cast a ray to the player to see if it is in view
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position), colliderMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
                playerIsInView = true;
            else
                playerIsInView = false; // if the player is being blocked by the environment then set the playerIsInView to false
        }
    }

    bool IsAbleToMove() // check if the Actor is able to move. Prerequisites being the Actor is not in range to attack, it is not staggered, and it is not attacking
    {
        return !isInRange && !isStaggered && !isAttacking;
    }

    void Attack() // attack the target by getting its Damage interface.
    {
        targetToDamage = target.GetComponent<IDamageable>();
        if (targetToDamage != null && !isAttacking) // if the target can be damaged and the actor isn't already attacking then call the Attack Animation
        {
            isAttacking = true;
            StartCoroutine(AttackAnimationTimer());
        }
    }

    public bool TakeDamage(int amount) // Called from a separate script controlled by the Actor causing damage
    {
        if (isAlive) // if the enemy is alive turn on the blood sprite, subtract from the current health, stagger and knockback the enemy
        {
            bloodSplatter[1].enabled = true;
            currentHealth -= amount;
            isStaggered = true;
            transform.Translate(-knockbackAmount, 0, 0);
            StartCoroutine(StaggerCoolDown());
            if (currentHealth <= 0) // if the enemy's health falls below 0 then turn off its collider
            {
                isAlive = false;
                col.enabled = false;
                if (dropper != null)
                    dropper.DropItem(); // calls the method DropItem from the actor's ItemDrop class.  This gives the method the items the actor is able to drop and the position to drop it at

                Vector3 deathPosition = transform.position;
                deathPosition.z = 1;
                transform.position = deathPosition;
                if (DeathEvent != null)
                    DeathEvent(gameObject);

                Invoke("Deactivate", 5f); // Turns the actor off after 5 seconds
                return true;
            }
        }
        return false;
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }

    IEnumerator StaggerCoolDown () // prevents the actor from moving based on the stagger time variable
    {
        yield return new WaitForSeconds(staggerTime);

        isStaggered = false;
        bloodSplatter[1].enabled = false;
    }

    IEnumerator AttackAnimationTimer () // waits for the attack speed specified before attacking
    {
        yield return new WaitForSeconds(attackSpeed);

        if (isInRange) // checks if the target is still in range
            if (targetToDamage.TakeDamage(attackDamage)) // calls the targets take damage method and returns whether or not the target died.
            {
                if (target != player)
                {
                    target = player;
                    isInRange = false;
                }
                else
                    playerIsDead = true;
            }


        isAttacking = false;
    }
}
