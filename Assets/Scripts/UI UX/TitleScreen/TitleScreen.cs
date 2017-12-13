using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
    const float MAX_IDLE_TIME = 120;

    public GameObject startScreen;
    public GameObject mainMenuScreen;
    public GameObject saveScreen;
    public GameObject background;
    public GameObject gameTitle;

    Vector3 mousePos;
    float idleTimer;
    bool onStartScreen;
    bool isIdle;

    void Start()
    {
        onStartScreen = true;
        idleTimer = 0;
        //SaveManager.data.ClearSaves();
        Button[] saveFiles = saveScreen.GetComponentsInChildren<Button>();
        HelperFunctions.PopulateSaveButtons(saveFiles);
    }

    void Update()
    {
        isIdle = true;
        if (Input.anyKeyDown && onStartScreen)
        {
            startScreen.SetActive(false);
            mainMenuScreen.SetActive(true);
            background.SetActive(true);
            gameTitle.SetActive(true);
            onStartScreen = false;
        }
        else if (!onStartScreen && isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= MAX_IDLE_TIME)
            {
                startScreen.SetActive(true);
                mainMenuScreen.SetActive(false);
                background.SetActive(false);
                gameTitle.SetActive(false);
                idleTimer = 0;
                onStartScreen = true;
            }
        }
        if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0 || Input.anyKeyDown)
        {
            isIdle = false;
            idleTimer = 0;
        }
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
