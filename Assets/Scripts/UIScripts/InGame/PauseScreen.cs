using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PauseScreen : MonoBehaviour {

    public event Action PausedEvent;

    bool paused;
    GameObject pauseScreen;

	void Start () {
        pauseScreen = gameObject.transform.GetChild(0).gameObject;
        pauseScreen.SetActive(false);
        paused = false;
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
        SceneManager.LoadScene("title screen");
    }

    void Pause()
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

    public void SendOutPauseEvent()
    {
        if (PausedEvent != null)
            PausedEvent();
    }
}
