using UnityEngine;

public class KeyItem : Interactable {

    public string itemName;

    string pickUpText;
    bool pickedUp;

    protected override void Interact()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        pickUpText = "You aquired a " + itemName;
        interactions.SetText(pickUpText);
        SaveManager.data.Items.Add(itemName);
        pickedUp = true;
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        if (pickedUp)
            gameObject.SetActive(false);
    }
}
