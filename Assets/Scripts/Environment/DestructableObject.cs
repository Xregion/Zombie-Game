using UnityEngine;

public class DestructableObject : MonoBehaviour, IDamageable {

    public int health;

    ItemDrop dropper;

    void Start()
    {
        dropper = GetComponent<ItemDrop>();
    }

    public bool TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
            if (dropper != null)
                dropper.DropItem();

            return true;
        }
        return false;
    }
}
