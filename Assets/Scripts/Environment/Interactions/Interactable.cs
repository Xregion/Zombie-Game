using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    enum PlayerPosition { north, south, east, west };
    public LayerMask playerMask;

    protected static InteractionText interactions;
    protected PlayerController player;
    protected bool interacting;

    float interactionDistance;

    void Awake()
    {
        interactions = GameObject.Find("Canvas").GetComponentInChildren<InteractionText>(true);
        interactionDistance = 3;
    }

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
    }

    void Update()
    {
        CheckForPlayer();
        if (Input.GetButtonDown("Interact") && interacting)
        {
            if (player.GetControls())
            {
                player.SetControls(false);
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
        if (hit.collider != null && hit.distance < interactionDistance)
        {
            if (CheckIfPlayerIsFacingObject(hit))
            {
                interactions.SetText("Press e to inspect.");
                interactions.EnableDialogue(true);
                interacting = true;
            }
            else
            {
                interacting = false;
                interactions.EnableDialogue(false);
            }
        }
        else
        {
            interacting = false;
            interactions.EnableDialogue(false);
        }
    }

    bool CheckIfPlayerIsFacingObject (RaycastHit2D hit)
    {
        Vector3 forward = hit.transform.TransformDirection(hit.transform.right).normalized;
        Vector3 dir = (transform.position - hit.transform.position).normalized;
        float dotProd = Vector2.Dot(forward, dir);

        print(dotProd);

        if (dotProd > 0.5)
            return true;

        return false;
        //PlayerPosition playerPosition = CheckPlayerPositionRelativeToInteractable(hit).GetValueOrDefault();
        //float playerRotation = hit.transform.rotation.eulerAngles.z;
        //switch (playerPosition)
        //{
        //    case PlayerPosition.east:
        //        if (playerRotation >= 135 && playerRotation <= 225)
        //            return true;
        //        break;
        //    case PlayerPosition.west:
        //        if (playerRotation >= 315 || playerRotation <= 45)
        //            return true;
        //        break;
        //    case PlayerPosition.north:
        //        if (playerRotation >= 225 && playerRotation <= 315)
        //            return true;
        //        break;
        //    case PlayerPosition.south:
        //        if (playerRotation >= 45 && playerRotation <= 135)
        //            return true;
        //        break;
        //    default:
        //        return false;
        //}

        //return false;
    }

    PlayerPosition? CheckPlayerPositionRelativeToInteractable(RaycastHit2D hit)
    {
        if (hit.transform.position.x > transform.position.x)
            return PlayerPosition.east;
        else if (hit.transform.position.x < transform.position.x)
            return PlayerPosition.west;
        else if (hit.transform.position.y > transform.position.y)
            return PlayerPosition.north;
        else if (hit.transform.position.y < transform.position.y)
            return PlayerPosition.south;
        else
            return null;
    }

    public virtual void StopInteracting()
    {
        player.SetControls(true);
        interactions.EnableDialogue(false);
        interacting = false;
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") && !interacting)
    //    {
    //        interactions.SetText("Press e to inspect.");
    //        interactions.EnableDialogue(true);
    //        interacting = true;
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
