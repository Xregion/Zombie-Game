using UnityEngine;
using UnityEngine.UI;
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
        SaveManager.data.XRotation = 0;
        SaveManager.data.YRotation = 0;
        SaveManager.data.ZRotation = 180;
        SaveManager.data.Bullets = 25;
        SaveManager.data.Items = new List<GameObject>();
        SaveManager.data.IsPowerOn = true;
        SaveManager.data.SaveData();
        SceneManager.LoadScene("Roof");
    }

    public void ContinueGame()
    {
        SaveManager.data.LoadData();
        print(SaveManager.data.Scene);
        SceneManager.LoadScene(SaveManager.data.Scene);
    }
}
