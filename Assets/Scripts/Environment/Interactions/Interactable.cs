using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected static InteractionText interactions;
    PlayerController player;
    bool interacting;

    void Awake()
    {
        interactions = FindObjectOfType<InteractionText>();
    }
     void Start()
    {
        player = LoadManager.instance.GetPlayer().GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && interacting)
        {
            if (player.GetControls())
            {
                player.SetControls(false);
                Interact();
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
        if (collision.CompareTag("Player") && player.GetControls())
        {
            interacting = false;
            interactions.EnableDialogue(false);
        }
    }

    protected abstract void Interact();
}
