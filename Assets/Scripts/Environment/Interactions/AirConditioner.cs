
public class AirConditioner : InspectableObject {

    void Awake()
    {
        if (!SaveManager.data.IsPowerOn)
            transform.GetChild(0).gameObject.SetActive(true);
    }
}
