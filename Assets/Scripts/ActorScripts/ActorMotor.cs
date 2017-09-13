using UnityEngine;

public class ActorMotor: MonoBehaviour {

    float rotationAngle;
    Vector3 target; // the target the actor should look at
    Vector3 actorRotation;

    void Start()
    {
        actorRotation = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (target != new Vector3(0, 0, 0))
            RotateActor();
    }

    // Move the Actor in the given direction and at the speed set in the Inspector
    public void MoveActor(Vector3 direction, float speed)
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    // Used to set the target to look at
    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    // Rotates the Actor to look at the set target
    void RotateActor()
    {
        rotationAngle = Mathf.Rad2Deg * Mathf.Acos((target.x - transform.position.x) / Mathf.Sqrt((Mathf.Pow(target.x - transform.position.x, 2) +
                Mathf.Pow(target.y - transform.position.y, 2))));

        if (target.y < transform.position.y)
            rotationAngle *= -1;

        actorRotation.z = rotationAngle;
        transform.eulerAngles = (actorRotation);
    }
}
