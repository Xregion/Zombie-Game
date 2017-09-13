using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected InteractionText interactions;
    bool interacting;

    void Start()
    {
        interactions = FindObjectOfType<InteractionText>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && interacting)
        {
            interacting = true;
            Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactions.SetText("Press e to inspect.");
            interactions.EnableDialogue(true);
            interacting = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interacting = false;
            interactions.EnableDialogue(false);
        }
    }

    protected abstract void Interact();
}
