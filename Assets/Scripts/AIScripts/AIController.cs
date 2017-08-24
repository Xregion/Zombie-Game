using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

    public GameObject actor;
    public GameObject[] items;
    public float maxHealth;
    public float movSpeed;
    public int attackDamage;
    public float attackSpeed;
    public float staggerTime;
    public float knockbackAmount;
    public float attackRange;
    public float dropChance;

    [HideInInspector]
    public bool isAlive;

    SpriteRenderer[] bloodSplatter;
    float enemyToPlayerAngle;
    float currentHealth;
    Vector3 rotation;
    PlayerController playerController;
    EnemySpawner spawner;
    BoxCollider2D col;
    bool isAttacking;
    bool isStaggered;

    void OnEnable()
    {
        isAlive = true;
    }

    void Start () {
        col = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        spawner = GetComponentInParent<EnemySpawner>();
        rotation = new Vector3 (0, 0, 0);
        bloodSplatter = GetComponentsInChildren<SpriteRenderer>(true);
        playerController = actor.GetComponent<PlayerController>();
        bloodSplatter[1].enabled = false;
	}
	
	void FixedUpdate () {
        if (isAlive)
        {
            if (actor.transform.position.y > transform.position.y)
                enemyToPlayerAngle = Mathf.Rad2Deg * Mathf.Acos((actor.transform.position.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(actor.transform.position.x - transform.position.x, 2) + 
                    Mathf.Pow(actor.transform.position.y - transform.position.y, 2))));
            else
                enemyToPlayerAngle = -Mathf.Rad2Deg * Mathf.Acos((actor.transform.position.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(actor.transform.position.x - transform.position.x, 2) + 
                    Mathf.Pow(actor.transform.position.y - transform.position.y, 2))));

            rotation.z = enemyToPlayerAngle;
            transform.eulerAngles = rotation;

            if (Vector3.Distance(actor.transform.position, transform.position) > attackRange && !isStaggered && !isAttacking)
                transform.Translate(new Vector3(movSpeed * Time.deltaTime, 0, 0));
            else if (Vector3.Distance(actor.transform.position, transform.position) < attackRange && !isStaggered)
                Attack();
        }
    }

    public void ReviveZombie()
    {
        currentHealth = maxHealth;
        isAlive = true;
        col.enabled = true;
    }

    void Attack()
    {
        if (playerController != null && !isAttacking)
        {
            isAttacking = true;
            playerController.TakeDamage(attackDamage);
            StartCoroutine(Cooldown(attackSpeed));
        }
    }

    public void TakeDamage (float damage)
    {
        if (isAlive)
        {
            bloodSplatter[1].enabled = true;
            currentHealth -= damage;
            isStaggered = true;
            transform.Translate(-knockbackAmount, 0, 0);
            StartCoroutine(StaggerCoolDown(staggerTime));
            if (currentHealth <= 0)
            {
                isAlive = false;
                col.enabled = false;
                float drop = Random.Range(0f, 1f);
                if (drop <= dropChance)
                    ItemDrop.DropItem(items, transform.position);

                Invoke("Destroy", 5f);
            }
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false);
        spawner.activeObjects.Remove(gameObject);
        CancelInvoke();
    }

    IEnumerator StaggerCoolDown (float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        isStaggered = false;
        bloodSplatter[1].enabled = false;
    }

    IEnumerator Cooldown (float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        isAttacking = false;
    }
}
