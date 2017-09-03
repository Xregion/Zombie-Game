using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ActorMotor))]
[RequireComponent(typeof(ItemDrop))]
public class AIController : MonoBehaviour, IDamageable {

    public event Action<GameObject> DeathEvent; // Called when zombie dies

    public GameObject[] items; //items that the AI can drop upon death
    public LayerMask environment;
    public float maxHealth;
    public float movSpeed;
    public int attackDamage;
    public float attackSpeed;
    public float staggerTime; // the amount of time after being shot that the AI can't move
    public float knockbackAmount; // the amount at x the AI will be knocked back when shot
    public float attackRange; // how far away the AI can hit the play
    public float dropChance;

    [HideInInspector]
    public bool isAlive;

    GameObject target;
    SpriteRenderer[] bloodSplatter;
    float enemyToPlayerAngle;
    float currentHealth;
    IDamageable targetToDamage;
    ItemDrop dropper;
    ActorMotor motor;
    BoxCollider2D col;
    bool isAttacking;
    bool isStaggered;
    bool isBlocked;

    void OnEnable()
    {
        isAlive = true;
    }

    void Start () {
        col = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        target = GameObject.Find("Player");
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
            CheckIfBlocked();
            motor.SetTarget(target.transform.position);

            if (Vector3.Distance(target.transform.position, transform.position) > attackRange && !isStaggered && !isAttacking && !isBlocked)
                motor.MoveActor(1, movSpeed);
            else if (/*Vector3.Distance(target.transform.position, transform.position) <= attackRange && */!isStaggered)
            {
                Attack();
            }
        }
    }

    public void ReviveZombie()
    {
        currentHealth = maxHealth;
        isAlive = true;
        col.enabled = true;
    }

    void CheckIfBlocked()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackRange, environment);
        if (hit.collider != null)
        {
            isBlocked = true;
            ChangeTargets(hit.collider.gameObject);
        }
        else
        {
            isBlocked = false;
            ChangeTargets(GameObject.Find("Player"));
        }
    }

    void Attack()
    {
        targetToDamage = target.GetComponent<IDamageable>();
        if (targetToDamage != null && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackAnimationTimer(attackSpeed));
        }
    }

    void ChangeTargets(GameObject newTarget)
    {
        target = newTarget;
    }

    public void TakeDamage(int amount)
    {
        if (isAlive)
        {
            bloodSplatter[1].enabled = true;
            currentHealth -= amount;
            isStaggered = true;
            transform.Translate(-knockbackAmount, 0, 0);
            StartCoroutine(StaggerCoolDown(staggerTime));
            if (currentHealth <= 0)
            {
                isAlive = false;
                col.enabled = false;
                float drop = UnityEngine.Random.Range(0f, 1f);
                if (drop <= dropChance)
                    dropper.DropItem(items, transform.position);

                if (DeathEvent != null)
                    DeathEvent(gameObject);

                Invoke("Deactivate", 5f);
            }
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }

    IEnumerator StaggerCoolDown (float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        isStaggered = false;
        bloodSplatter[1].enabled = false;
    }

    IEnumerator AttackAnimationTimer (float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);

        targetToDamage.TakeDamage(attackDamage);

        isAttacking = false;
    }
}
