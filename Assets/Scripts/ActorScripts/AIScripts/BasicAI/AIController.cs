using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActorMotor))]
[RequireComponent(typeof(ItemDrop))]
public class AIController : MonoBehaviour, IDamageable {

    public LayerMask colliderMask;
    public LayerMask attackingMask;
    public AudioClip playerSeen;
    public AudioClip damagedClip;
    public AudioClip idlingClip;
    public float maxHealth;
    public float movSpeed;
    public int attackDamage;
    public float attackRange;
    public float staggerTime; // the amount of time after being shot that the AI can't move
    public float knockbackAmount; // the amount at x the AI will be knocked back when shot

    bool isAlive;
    bool isPaused;
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
    protected static bool playerIsDead;
    PauseScreen ps;
    AudioSource audioSource;

    void OnEnable()
    {
        player = LoadManager.instance.GetPlayer();
        playerIsDead = false;
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
        audioSource = GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        ps.PausedEvent -= Pause;
    }

    void Update () {
        if (isAlive && !isPaused)
            Movement();
    }

    /// <summary>
    /// Handles the death of the player and sets the target variable. Must override to set the zombies movements.
    /// </summary>
    protected virtual void Movement ()
    {
        // if the player is in view then target him
        if ((CheckIfPlayerIsInView() && target != player) || target == null)
            target = player;

        if (playerIsDead && target == player)
        {
            animations.SetIsMeleeing(false);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
            if ((CheckIfInRange() || hit.collider.CompareTag("Enemy")))
            {
                //print(hit.collider.name);
                movSpeed = 0;
                animations.SetIsMoving(false);
                if (hit.collider.CompareTag("Player"))
                {
                    // Start player eating animation
                    animations.SetIsEating(true);
                    isBlocked = false;
                }
                else
                    isBlocked = true;
            }
        }
    }

    /// <summary>
    /// Brings the zombie back to life.
    /// </summary>
    public void Revive()
    {
        currentHealth = maxHealth;
        isAlive = true;
        col.enabled = true;
    }

    void Pause()
    {
        isPaused = !isPaused;
    }

    /// <summary>
    /// Checks if the player is in the line of sight of the zombie
    /// </summary>
    /// <returns>true if the player is not obscured and false if he is</returns>
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

    /// <summary>
    /// Checks if the player is in range for an attack.
    /// </summary>
    /// <returns>true if the player is in range and false if he isn't</returns>
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
            if (CheckIfInRange() && isAlive && isAttacking) // checks if the target is still in range
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
            StartCoroutine(PlayAudioClip(damagedClip, true));
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

    protected virtual void Die()
    {
        animations.SetIsDead(true);
        isAlive = false;
        col.enabled = false;
        if (dropper != null)
            dropper.DropItem(); // calls the method DropItem from the actor's ItemDrop class.  This gives the method the items the actor is able to drop and the position to drop it at

        //Vector3 deathPosition = transform.position;
        //deathPosition.z = 14.5f;
        //transform.position = deathPosition;
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Background";
        SaveManager.data.PlayerIsInCombat = false;
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

    /// <summary>
    /// Plays the specified clip after the current clip is done or interrupts the current clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="interrupt"></param>
    protected IEnumerator PlayAudioClip(AudioClip clip, bool interrupt)
    {
        bool isPlaying = audioSource.isPlaying;
        float currentClipTime = audioSource.time;
        AudioClip currentClip = audioSource.clip;
        if (interrupt)
        {
            audioSource.clip = clip;
            audioSource.Stop();
            audioSource.Play();
        }
        else
        {
            if (isPlaying)
                yield return new WaitForSeconds(currentClip.length - currentClipTime);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
