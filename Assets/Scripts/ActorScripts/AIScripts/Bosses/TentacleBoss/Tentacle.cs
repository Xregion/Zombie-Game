using UnityEngine;
using System.Collections;

public class Tentacle : MonoBehaviour, IDamageable {

    static TentacleBoss tentacleBoss;

    static bool isDead;

    void Start()
    {
        tentacleBoss = GetComponentInParent<TentacleBoss>();
        tentacleBoss.DeathEvent += Die;
    }

    void Die()
    {
        isDead = true;
        tentacleBoss.DeathEvent -= Die;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDead)
            tentacleBoss.DamagePlayer(collision);
    }

    public bool TakeDamage(int amount)
    {
        return tentacleBoss.TakeDamage(amount);
    }

    public IEnumerator MoveTentacle(int direction, float distance, float speed, float delay, bool isAttacking)
    {
        bool isMoving = true;
        float originalPosition = transform.localPosition.y;

        while (isMoving && !isDead)
        {
            transform.Translate(new Vector3(0, direction, 0) * speed * Time.deltaTime);
            float distanceMoved = Mathf.Abs(transform.localPosition.y - originalPosition);
            if (distanceMoved > distance)
            {
                if (transform.localPosition.y >= distance - 0.5)
                {
                    yield return new WaitForSeconds(delay);
                    StartCoroutine(MoveTentacle(-direction, distance, speed, delay, false));
                }
                isMoving = false;
            }
            yield return new WaitForEndOfFrame();
        }
        tentacleBoss.SetIsAttacking(isAttacking);
    }
}
