using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ActorMotor))]
[RequireComponent(typeof(ItemDrop))]
public class AIController : MonoBehaviour, IDamageable {

    public event Action<GameObject> DeathEvent; // Called when zombie dies

    public LayerMask colliderMask;
    public LayerMask attackingMask;
    public float maxHealth;
    public float movSpeed;
    public int attackDamage;
    public float attackRange;
    public float staggerTime; // the amount of time after being shot that the AI can't move
    public float knockbackAmount; // the amount at x the AI will be knocked back when shot

    bool isAlive;
    bool paused;
    protected GameObject target;
    protected GameObject player;
    protected float distanceToPlayer;
    SpriteRenderer[] bloodSplatter;
    protected EnemyAnimations animations;
    float currentHealth;
    IDamageable targetToDamage;
    ItemDrop dropper;
    protected ActorMotor motor;
    BoxCollider2D col;
    protected bool isAttacking;
    protected bool isStaggered;
    protected bool isBlocked;
    static bool playerIsDead;
    PauseScreen ps;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
        isAlive = true;
        col = GetComponent<BoxCollider2D>();
        animations = GetComponentInChildren<EnemyAnimations>();
        currentHealth = maxHealth;
        bloodSplatter = GetComponentsInChildren<SpriteRenderer>(true);
        dropper = GetComponent<ItemDrop>();
        motor = GetComponent<ActorMotor>();
        bloodSplatter[1].enabled = false;
        ps = FindObjectOfType<PauseScreen>();
        ps.PausedEvent += Pause;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
        ps.PausedEvent -= Pause;
    }

    void LoadComplete()
    {
        player = LoadManager.instance.GetPlayer();
    }

    void Update () {
        if (isAlive && !paused)
            Movement();
    }

    protected virtual void Movement ()
    {
        CheckIfPlayerIsInView();

        // if the player is in view then target him
        if ((CheckIfPlayerIsInView() && target != player) || target == null)
            target = player;

        if (playerIsDead && target == player)
        {
            animations.SetIsMeleeing(false);
            if ((CheckIfInRange() || isBlocked))
            {
                movSpeed = 0;
                animations.SetIsMoving(false);
                if (!isBlocked)
                {
                    // Start player eating animation
                    animations.SetIsEating(true);
                }
            }
        }
    }

    public void Revive()
    {
        currentHealth = maxHealth;
        isAlive = true;
        col.enabled = true;
    }

    void Pause()
    {
        paused = !paused;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            isBlocked = true;
    }

    protected bool CheckIfPlayerIsInView ()
    {
        // cast a ray to the player to see if it is in view
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position), colliderMask);
        if (hit.collider != null)
            if (hit.collider.CompareTag("Player"))
            {
                distanceToPlayer = hit.distance;
                return true;
            }

        return false;
    }

    protected bool CheckIfInRange ()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, attackRange, attackingMask);
        if (hit.collider != null)
        {
            target = hit.transform.gameObject;
            return true;
        }

        return false;
    }

    public void Attack() // attack the target by getting its Damage interface.
    {
        targetToDamage = target.GetComponent<IDamageable>();
        if (targetToDamage != null) // if the target can be damaged
        {
            attackRange += 0.75f;
            if (CheckIfInRange()) // checks if the target is still in range
            {
                if (targetToDamage.TakeDamage(attackDamage)) // calls the targets take damage method and returns whether or not the target died.
                {
                    if (target != player)
                        target = player;
                    else
                    {
                        animations.SetIsMeleeing(false);
                        playerIsDead = true;
                    }
                }
            }
            attackRange -= 0.75f;
        }
    }

    public bool TakeDamage(int amount) // Called from a separate script controlled by the Actor causing damage
    {
        if (isAlive) // if the enemy is alive turn on the blood sprite, subtract from the current health, stagger and knockback the enemy
        {
            if (isAttacking)
            {
                isAttacking = false;
                animations.SetIsMeleeing(false);
            }
            bloodSplatter[1].enabled = true;
            currentHealth -= amount;
            isStaggered = true;
            transform.Translate(-knockbackAmount, 0, 0);
            StartCoroutine(StaggerCoolDown());
            if (currentHealth <= 0) // if the enemy's health falls below 0 then turn off its collider
            {
                Die();
                return true;
            }
        }
        return false;
    }

    void Die()
    {
        animations.SetIsDead(true);
        isAlive = false;
        col.enabled = false;
        if (dropper != null)
            dropper.DropItem(); // calls the method DropItem from the actor's ItemDrop class.  This gives the method the items the actor is able to drop and the position to drop it at

        Vector3 deathPosition = transform.position;
        deathPosition.z = 14.5f;
        transform.position = deathPosition;
        if (DeathEvent != null)
            DeathEvent(gameObject);
    }

    public void SetIsAttacking ()
    {
        isAttacking = false;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    IEnumerator StaggerCoolDown () // prevents the actor from moving based on the stagger time variable
    {
        yield return new WaitForSeconds(staggerTime);

        isStaggered = false;
        bloodSplatter[1].enabled = false;
    }
}
