using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveScreen : MonoBehaviour {

    GameObject player;
    SaveConsole console;
    Image savePrompt;

    void Start()
    {
        player = LoadManager.instance.GetPlayer();
        savePrompt = GetComponentInChildren<Image>();
        Enable(false);
    }

    public void Enable(bool enabled)
    {
        savePrompt.gameObject.SetActive(enabled);
    }

    public void SetConsole(SaveConsole sc)
    {
        console = sc;
    }
    //TODO: Fix filepathing
    public void SaveGame ()
    {
        SaveManager.data.Scene = SceneManager.GetActiveScene().buildIndex;
        SaveManager.data.Health = player.GetComponent<PlayerController>().GetCurrentHealth();
        SaveManager.data.BulletsRemaining = player.GetComponentInChildren<GunController>().totalBulletsRemaining;
        SaveManager.data.BulletsInChamber = player.GetComponentInChildren<GunController>().GetBulletsInChamber();
        SaveManager.data.XPosition = player.transform.position.x;
        SaveManager.data.YPosition = player.transform.position.y;
        SaveManager.data.ZRotation = player.transform.rotation.z;
        SaveManager.data.PlayerIsInCombat = false;
        SaveManager.data.SaveData(1);
        Cancel();
    }

    public void Cancel ()
    {
        Enable(false);
        if (console != null)
            console.StopInteracting();
    }
}
