using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{

    public GameObject saveScreen;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropwdown;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    void Start()
    {
        //SaveManager.data.ClearSaves();
        Button[] saveFiles = saveScreen.GetComponentsInChildren<Button>();
        HelperFunctions.PopulateSaveButtons(saveFiles);

        // Set Volume Slider Value Based on Audio Mixer volume
        float volumeLevel = 0;
        audioMixer.GetFloat("volume", out volumeLevel);
        volumeSlider.value = volumeLevel;

        // Set quality dropdown value to current graphics quality level
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        // Set fullscreen toggle
        fullscreenToggle.isOn = Screen.fullScreen;

        // Set Resolutions Dropdown Options to the available resolutions
        resolutions = Screen.resolutions;
        resolutionDropwdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropwdown.AddOptions(options);
        resolutionDropwdown.value = currentResolutionIndex;
        resolutionDropwdown.RefreshShownValue();
    }

    public void NewGame()
    {
        SaveManager.data.CharacterName = "Kyle";
        SaveManager.data.Scene = SceneManager.GetActiveScene().buildIndex + 1;
        SaveManager.data.Health = 50;
        SaveManager.data.XPosition = 20;
        SaveManager.data.YPosition = -5;
        SaveManager.data.ZRotation = 180;
        SaveManager.data.BulletsRemaining = 60;
        SaveManager.data.BulletsInChamber = 12;
        SaveManager.data.Items = new List<string>();
        SaveManager.data.MannequinnWasShot = false;
        SaveManager.data.IsPowerOn = true;
        SaveManager.data.ZombieSpawnPoints = new Dictionary<SerializableVector3, bool>();
        SaveManager.data.TimePlayed = 0;
        SceneManager.LoadScene(SaveManager.data.Scene);
    }

    public void ContinueGame(int fileNumber)
    {
        if (SaveManager.data.LoadData(fileNumber))
            SceneManager.LoadScene(SaveManager.data.Scene);
        else
            print("No save data found");
    }

    public void LoadSave(int fileNumber)
    {
        if (SaveManager.data.LoadData(fileNumber))
            SceneManager.LoadScene(SaveManager.data.Scene);
        else
            print("No save data found");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
