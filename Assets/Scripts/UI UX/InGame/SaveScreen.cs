using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveScreen : MonoBehaviour
{
    public GameObject saveScreen;

    GameObject player;
    SaveConsole console;
    Image savePrompt;
    Button[] saveFiles;

    void Start()
    {
        player = LoadManager.instance.GetPlayer();
        savePrompt = GetComponentInChildren<Image>();
        Enable(false);
        saveFiles = saveScreen.GetComponentsInChildren<Button>();
        HelperFunctions.PopulateSaveButtons(saveFiles);
    }

    public void Enable(bool enabled)
    {
        savePrompt.gameObject.SetActive(enabled);
        if (!enabled)
            transform.GetChild(1).gameObject.SetActive(enabled);
    }

    public void SetConsole(SaveConsole sc)
    {
        console = sc;
    }

    public void SaveGame(int file)
    {
        SaveManager.data.Scene = SceneManager.GetActiveScene().buildIndex;
        SaveManager.data.Health = player.GetComponent<PlayerController>().GetCurrentHealth();
        SaveManager.data.BulletsRemaining = player.GetComponentInChildren<GunController>().totalBulletsRemaining;
        SaveManager.data.BulletsInChamber = player.GetComponentInChildren<GunController>().GetBulletsInChamber();
        SaveManager.data.XPosition = player.transform.position.x;
        SaveManager.data.YPosition = player.transform.position.y;
        SaveManager.data.ZRotation = player.transform.rotation.z;
        SaveManager.data.PlayerIsInCombat = false;
        SaveManager.data.SaveData(file);
        HelperFunctions.PopulateSaveButtons(saveFiles);
        Cancel();
    }

    public void Cancel()
    {
        Enable(false);
        if (console != null)
            console.StopInteracting();
    }
}
