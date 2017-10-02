public class InspectableObject : Interactable
{
    public string inspectionText;

    protected override void Interact()
    {
        interactions.SetText(inspectionText);
    }
}
