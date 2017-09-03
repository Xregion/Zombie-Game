using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour {

    public Image fillImage;
    public Color fullHealthColor;
    public Color zeroHealthColor;

    PlayerController actor;
    float currentHealth;
    Slider healthBar;

	void Start ()
    {
        healthBar = GetComponent<Slider>();
        actor = GetComponentInParent<Canvas>().gameObject.GetComponentInParent<PlayerController>();
        actor.HealthChangeEvent += HealthChange;
        actor.DeathEvent += PlayerDied;
        currentHealth = actor.currentHealth;
        healthBar.value = actor.totalHealth;
    }
	
	void HealthChange (float change)
    {
        healthBar.value += (change / actor.totalHealth);
        currentHealth = actor.currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / actor.totalHealth);
    }

    void PlayerDied ()
    {
        actor.HealthChangeEvent -= HealthChange;
        actor.DeathEvent -= PlayerDied;
    }
}
