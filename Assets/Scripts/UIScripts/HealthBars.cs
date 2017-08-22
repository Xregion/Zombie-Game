using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour {

    PlayerController actor;
    float currentHealth;
    Slider healthBar;

	void Start ()
    {
        healthBar = GetComponent<Slider>();
        actor = GetComponentInParent<Canvas>().gameObject.GetComponentInParent<PlayerController>();
        actor.DamageEvent += TookDamage;
        currentHealth = actor.currentHealth;
        healthBar.value = currentHealth;
    }
	
	void TookDamage (float damage)
    {
        healthBar.value -= (damage / actor.totalHealth);
    }
}
