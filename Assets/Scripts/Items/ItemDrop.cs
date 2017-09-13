using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public GameObject[] items;
    public float dropChance;

    public void DropItem ()
    {
        float drop = Random.Range(0f, 1f);
        if (drop <= dropChance)
        {
            int itemToDrop = Random.Range(0, items.Length);
            int amountToDrop = Random.Range(5, 11);

            GameObject item = Instantiate(items[itemToDrop], transform.position, Quaternion.identity);
            Item i = item.GetComponent<Item>();
            i.SetDropAmount(amountToDrop);
            i.StartDespawn();
        }
    }
}
