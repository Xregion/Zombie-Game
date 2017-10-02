using UnityEngine;
using UnityEngine.UI;

public class BulletsText : MonoBehaviour {

    GunController gunController;
    Text bulletText;

    void Start()
    {
        gunController = LoadManager.instance.GetPlayer().GetComponentInChildren<GunController>();
        bulletText = GetComponent<Text>();
    }

    void Update()
    {
        bulletText.text = gunController.GetBulletsInChamber() + "/" + gunController.totalBulletsRemaining;
    }
}
