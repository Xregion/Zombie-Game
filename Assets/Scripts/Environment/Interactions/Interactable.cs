using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public float interactionDistance;
    public LayerMask playerMask;

    protected static InteractionText interactions;
    protected PlayerController player;
    protected bool interacting;

    bool isFacingObject;
    PauseScreen pauseScreen;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
    }

    void LoadComplete()
    {
        player = LoadManager.instance.GetPlayer().GetComponent<PlayerController>();
        interactions = FindObjectOfType<InteractionText>();
        pauseScreen = FindObjectOfType<PauseScreen>();
    }

    void Update()
    {
        CheckForPlayer();
        if (Input.GetButtonDown("Interact") && interacting)
        {
            if (player.GetControls())
            {
                pauseScreen.SendOutPauseEvent();
                Interact();
            }
            else
            {
                StopInteracting();
            }
        }
    }

    void CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position), playerMask);
        if (hit.collider != null)
        {
            if (hit.distance < interactionDistance)
            {
                if (PlayerIsFacingObject(hit) && !interacting)
                {
                    interactions.SetText("Press e to inspect.");
                    interactions.EnableDialogue(true);
                    interacting = true;
                }
                else if (!PlayerIsFacingObject(hit) && interacting)
                {
                    StopInteracting();
                }
            }
            else if (interacting)
                StopInteracting();
        }
    }

    bool PlayerIsFacingObject(RaycastHit2D hit)
    {
        Vector3 dirToPlayer = (transform.position - hit.transform.position).normalized;
        float angleBetweenObjectAndPlayer = Vector3.Angle(hit.transform.right, dirToPlayer);

        float largestAngleAllowed = 45;

        if (angleBetweenObjectAndPlayer <= largestAngleAllowed)
            return true;

        return false;
    }

    public virtual void StopInteracting()
    {
        interactions.EnableDialogue(false);
        interacting = false;
        if (!player.GetControls())
            pauseScreen.SendOutPauseEvent();
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") && !interacting && isFacingObject)
    //    {
    //            interactions.SetText("Press e to inspect.");
    //            interactions.EnableDialogue(true);
    //            interacting = true;
    //    }
    //}

    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (PlayerIsFacingObject(collision.transform))
    //            isFacingObject = true;
    //        else
    //        {
    //            interacting = false;
    //            interactions.EnableDialogue(false);
    //            isFacingObject = false;
    //        }
    //    }

    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") && player.GetControls())
    //    {
    //        interacting = false;
    //        interactions.EnableDialogue(false);
    //    }
    //}

    protected abstract void Interact();
}
