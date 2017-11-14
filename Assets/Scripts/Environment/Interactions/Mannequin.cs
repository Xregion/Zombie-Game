using UnityEngine;

public class Mannequin : MonoBehaviour, IDamageable {

    InspectableObject io;
    GameObject mannequinHead;

    void Start()
    {
        mannequinHead = transform.GetChild(0).gameObject;
        mannequinHead.SetActive(false);
        io = GetComponent<InspectableObject>();
        if (SaveManager.data.MannequinnWasShot)
            io.StopAllInteractions();
    }

    public bool TakeDamage(int amount)
    {
        if (!SaveManager.data.MannequinnWasShot)
        {
            SaveManager.data.MannequinnWasShot = true;
            io.StopAllInteractions();
            mannequinHead.SetActive(true);
        }

        return true;
    }
}
