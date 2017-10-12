using UnityEngine;

public class KeyItem : Interactable {

    public string itemName;

    string pickUpText;

    protected override void Interact()
    {
        pickUpText = "You aquired a " + itemName;
        interactions.SetText(pickUpText);
        SaveManager.data.Items.Add(gameObject);
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        gameObject.SetActive(false);
    }
}
