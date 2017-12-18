using UnityEngine;

[RequireComponent (typeof(Doorway))]
public class LockedDoor : InspectableObject {

    public GameObject keyItem;
    public string hasKeyText;
    public bool isOneWayDoor;

    Doorway doorway;
    bool hasKeyItem;
    string keyItemName;

	void Start () {
        doorway = GetComponent<Doorway>();
        doorway.enabled = false;
        keyItemName = keyItem.GetComponent<KeyItem>().itemName;
    }

    protected override void Interact()
    {
        if (SaveManager.data.Items.Contains(keyItemName))
            hasKeyItem = true;

        if (hasKeyItem)
            inspectionText = hasKeyText;

        base.Interact();
    }

    public override void StopInteracting()
    {
        base.StopInteracting();

        if (hasKeyItem) {
            doorway.enabled = true;
            if (isOneWayDoor)
            {
                hasKeyItem = false;
                SaveManager.data.Items.Remove(keyItemName);
                FindObjectOfType<Inventory>().UpdateInventory();
            }
            enabled = false;
        }
    }
}
