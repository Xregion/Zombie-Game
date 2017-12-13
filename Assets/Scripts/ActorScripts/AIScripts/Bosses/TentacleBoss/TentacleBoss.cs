using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TentacleBoss : MonoBehaviour, IDamageable {

    public event Action DeathEvent;

    const float ENRAGE_PERCENT = 0.5f;
    //const float SPEED = 7.5f;
    const float MOVE_DISTANCE = 3.5f;
    const float TENTACLE_RETREAT_DELAY = 0.5f;
    const float DAMAGE_DELAY = 0.5f;
    const float MAX_HEALTH = 150;
    const string BOSS_NAME = "Tentacle Beast";

    public GameObject bossScreen;
    public GameObject blockingTentacle;

    float currentHealth = MAX_HEALTH;
    float speed = 7.5f;
    int attackPower = 5;
    int numOfTentaclesToAttack;
    bool isEnraged;
    bool isAttacking;
    bool playerIsDead;
    bool isDead;
    bool canDamagePlayer = true;
    AudioSource audioSource;
    Tentacle[] tentacles;
    Slider healthSlider;
    GameObjectShake shaker;

	void Start ()
    {
        shaker = GetComponent<GameObjectShake>();
        if (!SaveManager.data.IsTentacleBossDead)
        {
            bossScreen.SetActive(true);
            bossScreen.GetComponentInChildren<Text>().text = BOSS_NAME;
            healthSlider = bossScreen.GetComponentInChildren<Slider>();
            healthSlider.value = currentHealth / MAX_HEALTH;
            numOfTentaclesToAttack = 3;
            tentacles = GetComponentsInChildren<Tentacle>();
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
            int tentacle = UnityEngine.Random.Range(0, tentacles.Length);
            while (tentaclesChosen.Contains(tentacle))
                tentacle = UnityEngine.Random.Range(0, tentacles.Length);

            tentaclesChosen.Add(tentacle);
            StartCoroutine(tentacles[tentacle].MoveTentacle(1, MOVE_DISTANCE, speed, TENTACLE_RETREAT_DELAY, true));
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public bool TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth / MAX_HEALTH;
        if (currentHealth / MAX_HEALTH <= ENRAGE_PERCENT && !isEnraged)
        {
            isEnraged = true;
            attackPower *= 2;
            speed = 6f;
            numOfTentaclesToAttack = 5;
            audioSource.Play();
        }
        if (currentHealth <= 0)
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
        if (collision.collider.CompareTag("Player") && !isDead)
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
        if (DeathEvent != null)
            DeathEvent();

        healthSlider.fillRect = null;
        isDead = true;
        SaveManager.data.IsTentacleBossDead = true;
        audioSource.Play();
        shaker.StartShake(audioSource.clip.length);
        yield return new WaitWhile(() => audioSource.isPlaying);
        blockingTentacle.SetActive(false);
        gameObject.SetActive(false);
        bossScreen.SetActive(false);
    }

    public float GetBossHealth()
    {
        return currentHealth / MAX_HEALTH;
    }
}
