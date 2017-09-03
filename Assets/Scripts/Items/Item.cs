using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    int amountToDrop;
    float currentSecondsLeftOnDespawn = 0;
    float secondsUntilDespawnBegins = 10;
    float changeSpeed = 12;
    float secondsUntilDespawnEnds = 15;
    float blinkSpeed = 0.5f;
    float oldBlinkSpeed;
    SpriteRenderer ren;
    Color color;

    void OnEnable()
    {
        StartCoroutine(Despawn(secondsUntilDespawnBegins));
    }

    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        color = ren.color;
        oldBlinkSpeed = blinkSpeed;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator Despawn (float seconds)
    {
        yield return new WaitForSeconds(seconds);

        currentSecondsLeftOnDespawn = seconds;
        StartCoroutine(Blink(blinkSpeed));
    }

    IEnumerator Blink (float blinkSpeed)
    {
        color.a = 0;
        ren.color = color;
        yield return new WaitForSeconds(blinkSpeed);
        color.a = 1;
        ren.color = color;

        currentSecondsLeftOnDespawn += blinkSpeed;
        if (currentSecondsLeftOnDespawn >= changeSpeed && this.blinkSpeed == oldBlinkSpeed)
        {
            this.blinkSpeed /= 2f;
            oldBlinkSpeed = this.blinkSpeed;
            changeSpeed += 1f;
        }
        else if (currentSecondsLeftOnDespawn >= secondsUntilDespawnEnds)
        {
            StopAllCoroutines();
            Destroy();
        }

        yield return new WaitForSeconds(blinkSpeed);

        StartCoroutine(Blink(this.blinkSpeed));
    }

    public int GetAmountToDrop()
    {
        return amountToDrop;
    }

    public void SetDropAmount(int amountToDrop)
    {
        this.amountToDrop = amountToDrop;
    }
}
