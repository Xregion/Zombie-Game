using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public GameObject[] items;
    [Range(0, 1)]
    public float dropChance;
    [Tooltip("Set to 0 if you want a random number")]
    public int dropAmount;

    /// <summary>
    /// Drops a random item from the items array.
    /// </summary>
    public void DropRandomItem()
    {
        float drop = Random.Range(0f, 1f);
        if (drop <= dropChance)
        {
            int itemToDrop = Random.Range(0, items.Length);
            int amountToDrop;
            if (dropAmount == 0)
                amountToDrop = Random.Range(10, 21);
            else
                amountToDrop = dropAmount;

            GameObject item = Instantiate(items[itemToDrop], transform.position, Quaternion.identity);
            Item i = item.GetComponent<Item>();
            i.SetDropAmount(amountToDrop);
            i.StartDespawn();
        }
    }

    /// <summary>
    /// Drops the specificed item from the items array.
    /// </summary>
    /// <param name="item"></param>
    public void DropItem (GameObject item)
    {
        int amountToDrop;
        if (dropAmount == 0)
            amountToDrop = Random.Range(10, 21);
        else
            amountToDrop = dropAmount;

        GameObject droppedItem = Instantiate(item, transform.position, Quaternion.identity);
        Item i = droppedItem.GetComponent<Item>();
        i.SetDropAmount(amountToDrop);
        i.StartDespawn();
    }

    /// <summary>
    /// Returns the GameObject from the items list with the same name as "itemName"
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public GameObject GetItem (string itemName)
    {
        foreach (GameObject go in items)
        {
            if (go.name == itemName)
                return go;
        }

        return null;
    }
}
