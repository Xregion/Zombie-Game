using UnityEngine;

public class GameObjectShake : MonoBehaviour {

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    // How long the object should shake for.
    float shakeDuration = 0f;

    Vector3 originalPos;

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    public void StartShake(float duration)
    {
        shakeDuration = duration;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }
}
