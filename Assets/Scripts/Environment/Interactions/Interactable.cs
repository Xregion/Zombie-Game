using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public float interactionDistance;

    protected static InteractionText interactions;
    protected PlayerController player;
    protected bool canInteract; // Use this to check if the player is able to interact with the object
    protected bool isPointlessToInteract; // Checks if the object still has a use for interactions

    PauseScreen pauseScreen;
    MoveableObject moveableObject;
    int playerMask;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
        if (moveableObject != null)
            moveableObject.FinishedMoving -= StopAllInteractions;
    }

    void LoadComplete()
    {
        player = LoadManager.instance.GetPlayer().GetComponent<PlayerController>();
        interactions = FindObjectOfType<InteractionText>();
        pauseScreen = FindObjectOfType<PauseScreen>();
        playerMask = 1 << LayerMask.NameToLayer("Player");
        moveableObject = GetComponent<MoveableObject>();
        if (moveableObject != null)
            moveableObject.FinishedMoving += StopAllInteractions;
    }

    void Update()
    {
        if (!isPointlessToInteract)
        {
            CheckForPlayer();
            if (Input.GetButtonDown("Interact") && canInteract)
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
        else if (canInteract)
        {
            StopInteracting();
        }
    }

    void CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position), playerMask);
        if (hit.collider != null)
        {
            if (hit.distance <= interactionDistance)
            {
                if (PlayerIsFacingObject(hit) && !canInteract)
                {
                    interactions.SetText("Press e to inspect.");
                    interactions.EnableDialogue(true);
                    canInteract = true;
                }
                else if (!PlayerIsFacingObject(hit) && canInteract)
                {
                    StopInteracting();
                }
            }
            else if (canInteract)
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
        canInteract = false;
        if (!player.GetControls())
            pauseScreen.SendOutPauseEvent();
    }

    void StopAllInteractions()
    {
        isPointlessToInteract = true;
    }

    protected abstract void Interact();
}
