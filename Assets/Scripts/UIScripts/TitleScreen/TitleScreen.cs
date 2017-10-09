using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
    public void NewGame()
    {
        SaveManager.data.Scene = 1;
        SaveManager.data.Health = 50;
        SaveManager.data.XPosition = 20;
        SaveManager.data.YPosition = -5;
        SaveManager.data.ZRotation = 180;
        SaveManager.data.BulletsRemaining = 25;
        SaveManager.data.BulletsInChamber = 5;
        SaveManager.data.Items = new List<GameObject>();
        SaveManager.data.IsPowerOn = true;
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
}
