using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected InteractionText interactions;
    PlayerController player;
    bool interacting;

    void Awake()
    {
        interactions = FindObjectOfType<InteractionText>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && interacting)
        {
            if (player.GetControls())
            {
                Interact();
                player.SetControls(false);
            }
            else
            {
                player.SetControls(true);
                interactions.EnableDialogue(false);
                interacting = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !interacting)
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
