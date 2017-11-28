using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveScreen : MonoBehaviour {

    GameObject player;
    SaveConsole console;
    Image savePrompt;
    Button[] saveFiles;

    void Start()
    {
        player = LoadManager.instance.GetPlayer();
        savePrompt = GetComponentInChildren<Image>();
        Enable(false);
        saveFiles = transform.GetChild(1).GetComponentsInChildren<Button>();
    }

    public void Enable(bool enabled)
    {
        savePrompt.gameObject.SetActive(enabled);
        if (!enabled)
            transform.GetChild(1).gameObject.SetActive(enabled);
        //else
        //    PopulateSaveButtons();
    }

    public void SetConsole(SaveConsole sc)
    {
        console = sc;
    }

    public void SaveGame (int file)
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
        //PopulateSaveButtons();
        Cancel();
    }

    public void Cancel ()
    {
        Enable(false);
        if (console != null)
            console.StopInteracting();
    }

    void PopulateSaveButtons()
    {
        int currentFile = SaveManager.data.LoadedFile;
        if (currentFile != 0)
            saveFiles[currentFile - 1].GetComponentInChildren<Text>().text = SaveManager.data.CharacterName + " - " + SceneManager.GetSceneByBuildIndex(SaveManager.data.Scene).name;
        for (int i = 0; i < saveFiles.Length; i++)
        {
            if (i == currentFile - 1)
                continue;
            else
            {
                if (SaveManager.data.LoadData(i + 1))
                    saveFiles[i].GetComponentInChildren<Text>().text = SaveManager.data.CharacterName + " - " + SceneManager.GetSceneByBuildIndex(SaveManager.data.Scene).name;
                else
                    continue;
            }
        }
        SaveManager.data.LoadData(currentFile);
    }
}
