﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
    public void NewGame()
    {
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
