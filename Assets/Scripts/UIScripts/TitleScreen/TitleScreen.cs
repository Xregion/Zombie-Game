using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
    public void NewGame()
    {
        SaveManager.data.Scene = 1;
        SaveManager.data.XPosition = 20;
        SaveManager.data.YPosition = -5;
        SaveManager.data.ZPosition = 1;
        SaveManager.data.ZRotation = 180;
        SaveManager.data.Bullets = 25;
        SaveManager.data.Items = new List<GameObject>();
        SaveManager.data.IsPowerOn = true;
        SceneManager.LoadScene("Roof");
    }

    public void ContinueGame()
    {
        if (SaveManager.data.LoadData())
            SceneManager.LoadScene(SaveManager.data.Scene);
        else
            print("No save data found");
    }
}
