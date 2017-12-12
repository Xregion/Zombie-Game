using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class OptionsScreen : MonoBehaviour {
    
    public AudioMixer audioMixer;
    public Dropdown resolutionDropwdown;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    void Start ()
    {
        // Set Volume Slider Value Based on Audio Mixer volume
        float audioMixerLevel = 0;
        audioMixer.GetFloat("volume", out audioMixerLevel);
        float volumeLevel = PlayerPrefs.GetFloat("Volume Level", audioMixerLevel);
        volumeSlider.value = volumeLevel;
        audioMixer.SetFloat("volume", volumeLevel);

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
            //if (!options.Contains(option))
                options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropwdown.AddOptions(options);
        resolutionDropwdown.value = currentResolutionIndex;
        resolutionDropwdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume Level", volume);
        PlayerPrefs.Save();
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
}
