using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PauseScreen : MonoBehaviour {

    public event Action PausedEvent;

    bool paused;
    bool canPause;
    GameObject pauseScreen;

	void Start () {
        pauseScreen = transform.GetChild(0).gameObject;
        pauseScreen.SetActive(false);
        paused = false;
        canPause = true;
    }
	
	void Update () {
		if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
	}

    public void Continue()
    {
        Pause();
    }

    public void Quit()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadFromSave()
    {
        //TODO: IMPLEMENT FILE NUMBER TO GET FROM SAVEMANAGER
        int fileNumber = 1;
        if (SaveManager.data.LoadData(fileNumber))
            SceneManager.LoadScene(SaveManager.data.Scene);
    }

    void Pause()
    {
        if (canPause)
        {
            paused = !paused;
            if (paused)
            {
                pauseScreen.SetActive(true);
            }
            else
            {
                pauseScreen.SetActive(false);
            }

            SendOutPauseEvent();
        }
    }

    public void SendOutPauseEvent()
    {
        if (PausedEvent != null)
            PausedEvent();
    }

    public void EnablePause (bool enabled)
    {
        canPause = enabled;
    }
}
