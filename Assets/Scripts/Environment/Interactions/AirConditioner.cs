using UnityEngine;

public class AirConditioner : InspectableObject {

    public GameObject elevatorKey;

    void Awake()
    {
        if (!SaveManager.data.IsPowerOn)
        {
            GetComponent<Collider2D>().enabled = false;
            elevatorKey.SetActive(true);
        }
    }
}
