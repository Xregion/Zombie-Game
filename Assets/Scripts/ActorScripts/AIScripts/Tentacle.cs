using UnityEngine;
using System.Collections;

public class Tentacle : MonoBehaviour, IDamageable {

    const float ENRAGE_PERCENT = 0.25f;
    const float SPEED = 5;

    static int maxHealth = 100;
    static int health = 100;
    static bool enraged;

    static Tentacle[] tentacles;
    static int numOfTentaclesToAttack;

	void Start () {
        numOfTentaclesToAttack = 1;
        tentacles = transform.parent.GetComponentsInChildren<Tentacle>();
        StartCoroutine(SwingTentacle());
	}

    IEnumerator SwingTentacle()
    {
        float waitTime = 1f;
        int tentacleIndex = -1;
        while (health > 0)
        {
            for (int i = 0; i < numOfTentaclesToAttack; i++)
            {
                int oldIndex = tentacleIndex;
                tentacleIndex = Random.Range(0, tentacles.Length);
                if (oldIndex == tentacleIndex)
                {
                    if (tentacleIndex == 0)
                        tentacleIndex++;
                    else if (tentacleIndex == tentacles.Length - 1)
                        tentacleIndex--;
                }
                MoveTentacle(tentacleIndex, 1);
            }

            if (maxHealth / health <= ENRAGE_PERCENT && !enraged)
            {
                enraged = true;
                waitTime /= 2;
                numOfTentaclesToAttack = 2;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    void MoveTentacle(int tentacle, int direction)
    {
        while (tentacles[tentacle].transform.position.z > 0)
        {
            tentacles[tentacle].transform.Translate(new Vector3(0, 0, 1) * SPEED * Time.deltaTime);
            if (tentacles[tentacle].transform.position.z < 0) {
                Vector3 pos = tentacles[tentacle].transform.position;
                pos.z = 0;
                tentacles[tentacle].transform.position = pos;
                //MoveTentacle(tentacle, -1);
            }
        }
    }

    public bool TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    void Die()
    {

    }
}
