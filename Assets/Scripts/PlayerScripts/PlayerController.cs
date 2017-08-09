using UnityEngine;

[RequireComponent(typeof(GunController))]
public class PlayerController : MonoBehaviour {

    public float movSpeed;
    public float currentHealth;
    public float totalHealth;

    float horizontalDirection;
    float verticalDirection;
    float mouseToPlayerAngle;
    Rigidbody2D rb;
    Vector3 mousePosition;
    Vector3 playerRotation;
    bool controlsOn;
    GunController gunController;

    void Start()
    {
        playerRotation = new Vector3(0, 0, 0);
        gunController = GetComponent<GunController>();
        controlsOn = true;
        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update()
    {
        if (controlsOn)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                gunController.Fire();
            }
            if (Input.GetAxis("Reload") != 0 && !gunController.reloading)
            {
                gunController.Reload();
            }
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void FixedUpdate()
    {
        if (controlsOn)
        {
            // Have player look at cursor at all times
            mousePosition = Input.mousePosition;
            mousePosition.z = transform.position.z;
            Vector3 worldPointMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (worldPointMousePosition.y > transform.position.y)
            {
                mouseToPlayerAngle = Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
            }
            else
            {
                mouseToPlayerAngle = -Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
            }
            playerRotation.z = mouseToPlayerAngle;
            transform.eulerAngles = (playerRotation);

            horizontalDirection = Input.GetAxis("Horizontal");
            verticalDirection = Input.GetAxis("Vertical");
            if (horizontalDirection != 0)
            {
                Strafe();
            }
            if (verticalDirection != 0)
            {
                MoveCharacter();
            }
        }
    }

    void MoveCharacter()
    {
        transform.Translate(new Vector3(verticalDirection * movSpeed * Time.deltaTime, 0, 0));
    }

    void Strafe()
    {
        transform.Translate(new Vector3(0, -horizontalDirection * movSpeed * Time.deltaTime, 0));
    }

    void Die()
    {
        controlsOn = false;
    }

    //void RotatePlayer()
    //{
    //    mousePosition = Input.mousePosition;
    //    mousePosition.z = transform.position.z;
    //    Vector3 worldPointMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //    if (worldPointMousePosition.y > transform.position.y)
    //    {
    //        mouseToPlayerAngle = Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
    //    }
    //    else
    //    {
    //        mouseToPlayerAngle = -Mathf.Rad2Deg * Mathf.Acos((worldPointMousePosition.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(worldPointMousePosition.x - transform.position.x, 2) + Mathf.Pow(worldPointMousePosition.y - transform.position.y, 2))));
    //    }
    //    playerRotation.z = mouseToPlayerAngle;
    //    transform.eulerAngles = (playerRotation);
    //}
}
