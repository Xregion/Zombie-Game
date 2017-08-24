using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public static void DropItem (GameObject[] items, Vector3 position)
    {
        int itemToDrop = Random.Range(0, items.Length);
        int amountToDrop = Random.Range(1, 11);

        Instantiate(items[itemToDrop], position, Quaternion.identity);
        items[itemToDrop].GetComponent<IDroppable>().SetDropAmount(amountToDrop);
    }
}
