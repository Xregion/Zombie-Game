using UnityEngine;
using System.Collections;

public class FlameFlicker : MonoBehaviour {

    SpriteRenderer ren;

	void Start () {
        ren = GetComponent<SpriteRenderer>();
        StartCoroutine(Flicker());
	}

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(0.5f);
        ren.flipX = !ren.flipX;
        StartCoroutine(Flicker());
    }
}
