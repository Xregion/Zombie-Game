using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreen : MonoBehaviour {

    const float FADE_SPEED = 0.003f;

    GameObject player;
    GameObject panel;

    void Start()
    {
        player = LoadManager.instance.GetPlayer();
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
        player.GetComponent<PlayerController>().DeathEvent += PlayerDied;
    }

    void PlayerDied()
    {
        panel.SetActive(true);
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image i in images)
        {
            StartCoroutine(FadeIn(i));
        }
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text t in texts)
        {
            StartCoroutine(FadeIn(t));
        }
    }

    IEnumerator FadeIn(Image i)
    {
        float maxAlpha = i.color.a;
        Color color = i.color;
        color.a = 0;
        i.color = color;
        while (i.color.a < maxAlpha)
        {
            color.a += FADE_SPEED;
            i.color = color;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeIn(Text t)
    {
        float maxAlpha = t.color.a;
        Color color = t.color;
        color.a = 0;
        t.color = color;
        while (t.color.a < maxAlpha)
        {
            color.a += FADE_SPEED;
            if (color.a > maxAlpha)
                color.a = maxAlpha;
            t.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}
