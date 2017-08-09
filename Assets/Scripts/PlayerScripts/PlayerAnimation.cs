using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    Animator animator;
    bool isMoving;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool fire = Input.GetButtonDown("Fire1");

        if (h != 0 || v != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("Moving", isMoving);
        animator.SetBool("Fire", fire);
    }
}
