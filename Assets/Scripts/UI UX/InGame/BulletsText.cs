using UnityEngine;
using UnityEngine.UI;

public class BulletsText : MonoBehaviour {

    GunController gunController;
    Text bulletText;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
    }

    void LoadComplete()
    {
        gunController = LoadManager.instance.GetPlayer().GetComponentInChildren<GunController>();
    }

    void Start()
    {
        bulletText = GetComponent<Text>();
    }

    void Update()
    {
        bulletText.text = gunController.GetBulletsInChamber() + "/" + gunController.totalBulletsRemaining;
    }
}
