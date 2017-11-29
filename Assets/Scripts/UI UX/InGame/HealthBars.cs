using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour {

    public Image fillImage;
    public Color fullHealthColor;
    public Color zeroHealthColor;

    PlayerController actor;
    Slider healthBar;

	void Awake ()
    {
        healthBar = GetComponent<Slider>();
        actor = GetComponentInParent<Canvas>().gameObject.GetComponentInParent<PlayerController>();
        actor.HealthChangeEvent += HealthChange;
        actor.DeathEvent += PlayerDied;
        HealthChange();
    }
	
	void HealthChange ()
    {
        float currentHealth = actor.GetCurrentHealth();
        healthBar.value = currentHealth / actor.totalHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / actor.totalHealth);
    }

    void PlayerDied ()
    {
        actor.HealthChangeEvent -= HealthChange;
        actor.DeathEvent -= PlayerDied;
    }
}
