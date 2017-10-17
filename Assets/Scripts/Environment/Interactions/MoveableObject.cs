using UnityEngine;
using System;

public class MoveableObject : MonoBehaviour
{
    public event Action FinishedMoving;

    public enum Direction { NORTH, SOUTH, EAST, WEST, ALL };

    public float moveDistance; // set to Infinity for never ending movement
    public float speed; // the speed at which the object can be moved
    public Direction direction; // the direction(s) the object can be moved

    Vector2 originalPosition;
    Vector2 newPosition;

    void Start()
    {
        originalPosition = transform.position;
        newPosition = originalPosition;
    }

    public void Move(Vector3 direction)
    {
        // make directions exactly 1 or -1 and don't allow movement unless moving in the allowed direction
        switch (this.direction) {
            case Direction.NORTH:
                if (Mathf.RoundToInt(direction.y) != 1)
                    return;
                else
                    direction = new Vector3(0, 1, 0);
                break;
            case Direction.SOUTH:
                if (Mathf.RoundToInt (direction.y) != -1)
                    return;
                else
                    direction = new Vector3(0, -1, 0);
                break;
            case Direction.WEST:
                if (Mathf.RoundToInt(direction.x) != -1)
                    return;
                else
                    direction = new Vector3(-1, 0, 0);
                break;
            case Direction.EAST:
                if (Mathf.RoundToInt(direction.x) != 1)
                    return;
                else
                    direction = new Vector3(1, 0, 0);
                break;
            case Direction.ALL:
                Vector3 roundedDirection = new Vector3
                {
                    x = Mathf.Round(direction.x),
                    y = Mathf.Round(direction.y),
                    z = 0
                };
                if (roundedDirection.x > roundedDirection.y)
                    roundedDirection.y = 0;
                else
                    roundedDirection.x = 0;
                direction = roundedDirection.normalized;
                break;
            default:
                return;
        }


        if (Vector2.Distance(originalPosition, newPosition) < moveDistance)
        {
            transform.Translate(direction * speed * Time.deltaTime);

            newPosition = transform.position;
        }
        else
        {
            if (FinishedMoving != null)
                FinishedMoving();
        }
    }
}
