using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TentacleBoss : MonoBehaviour, IDamageable {

    public event Action deathEvent;

    const float ENRAGE_PERCENT = 0.5f;
    const float SPEED = 5f;
    const float MOVE_DISTANCE = 3.5f;
    const float TENTACLE_RETREAT_DELAY = 0.5f;
    const float DAMAGE_DELAY = 0.5f;
    const float MAX_HEALTH = 150;

    public GameObject bossScreen;
    public GameObject blockingTentacle;

    float health = MAX_HEALTH;
    int attackPower = 5;
    int numOfTentaclesToAttack;
    bool isEnraged;
    bool isAttacking;
    bool playerIsDead;
    bool isDead;
    bool canDamagePlayer = true;
    AudioSource audioSource;
    Transform[] tentacles;
    Slider healthSlider;
    GameObjectShake shaker;

	void Start ()
    {
        shaker = GetComponent<GameObjectShake>();
        if (!SaveManager.data.IsTentacleBossDead)
        {
            bossScreen.SetActive(true);
            bossScreen.GetComponentInChildren<Text>().text = "Tentacle Beast";
            healthSlider = bossScreen.GetComponentInChildren<Slider>();
            healthSlider.value = health / MAX_HEALTH;
            numOfTentaclesToAttack = 2;
            tentacles = GetComponentsInChildren<Transform>();
            audioSource = GetComponent<AudioSource>();
            ChooseTentacle();
        }
        else
        {
            gameObject.SetActive(false);
            blockingTentacle.SetActive(false);
            isDead = true;
        }
	}

    void Update ()
    {
        if (!isAttacking && !playerIsDead && !isDead)
            ChooseTentacle();
    }

    void ChooseTentacle ()
    {
        List<int> tentaclesChosen = new List<int>();
        isAttacking = true;
        for (int i = 0; i < numOfTentaclesToAttack; i++)
        {
            int tentacle = UnityEngine.Random.Range(1, tentacles.Length);
            while (tentaclesChosen.Contains(tentacle))
                tentacle = UnityEngine.Random.Range(1, tentacles.Length);

            tentaclesChosen.Add(tentacle);
            tentacles[tentacle].GetComponent<Tentacle>().StartMovingTentacle(1, MOVE_DISTANCE, SPEED, TENTACLE_RETREAT_DELAY, true);
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public bool TakeDamage(int amount)
    {
        health -= amount;
        healthSlider.value = health / MAX_HEALTH;
        if (health / MAX_HEALTH <= ENRAGE_PERCENT && !isEnraged)
        {
            isEnraged = true;
            attackPower *= 2;
            numOfTentaclesToAttack *= 2;
            audioSource.Play();
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
            return true;
        }

        return false;
    }

    public void DamagePlayer(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && canDamagePlayer && !playerIsDead)
        {
            if (damageable.TakeDamage(attackPower))
                playerIsDead = true;

            canDamagePlayer = false;
            StartCoroutine(DelayDamageTimer());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            DamagePlayer(collision);
    }

    IEnumerator DelayDamageTimer()
    {
        float timeOfImpact = Time.time;
        while (Time.time - timeOfImpact < DAMAGE_DELAY)
            yield return new WaitForEndOfFrame();

        canDamagePlayer = true;
    }

    IEnumerator Die()
    {
        if (deathEvent != null)
            deathEvent();

        SaveManager.data.IsTentacleBossDead = true;
        audioSource.Play();
        shaker.StartShake(audioSource.clip.length);
        yield return new WaitWhile(() => audioSource.isPlaying);
        blockingTentacle.SetActive(false);
        isDead = true;
        gameObject.SetActive(false);
        healthSlider.fillRect = null;
        bossScreen.SetActive(false);
    }

    public float GetBossHealth()
    {
        return health / MAX_HEALTH;
    }
}
