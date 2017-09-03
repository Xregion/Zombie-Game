using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public void DropItem (GameObject[] items, Vector3 position)
    {
        int itemToDrop = Random.Range(0, items.Length);
        int amountToDrop = Random.Range(5, 11);

        GameObject item = Instantiate(items[itemToDrop], position, Quaternion.identity);
        item.GetComponent<Item>().SetDropAmount(amountToDrop);
    }
}
