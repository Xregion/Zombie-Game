using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScreen : MonoBehaviour {

    GameObject player;
    SaveConsole console;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
    }

    void LoadComplete()
    {
        player = LoadManager.instance.GetPlayer();
    }

    void Start()
    {
        Enable(false);
    }

    public void Enable(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void SetConsole(SaveConsole sc)
    {
        console = sc;
    }

    public void SaveGame ()
    {
        SaveManager.data.Scene = SceneManager.GetActiveScene().buildIndex;
        SaveManager.data.Bullets = player.GetComponentInChildren<GunController>().totalBulletsRemaining;
        SaveManager.data.XPosition = player.transform.position.x;
        SaveManager.data.YPosition = player.transform.position.y;
        SaveManager.data.ZPosition = player.transform.position.z;
        SaveManager.data.ZRotation = player.transform.rotation.z;
        SaveManager.data.SaveData();
        Cancel();
    }

    public void Cancel ()
    {
        Enable(false);
        if (console != null)
            console.StopInteracting();
    }
}
