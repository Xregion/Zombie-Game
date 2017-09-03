using UnityEngine;

public class DestructableObject : MonoBehaviour, IDamageable {

    public int health;

    public bool TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
