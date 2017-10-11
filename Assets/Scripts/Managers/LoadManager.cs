using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadManager : MonoBehaviour {

    public static LoadManager instance;

    public event Action LevelLoaded;

    public GameObject player;
    public GameObject zombie;
    GameObject instantiatedPlayer;

    void Awake()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    private void SceneChange(Scene previousScene, Scene loadedScene)
    {
        DestroyMultipleInstances();
        if (loadedScene != SceneManager.GetSceneByName("title screen"))
            instantiatedPlayer = Instantiate(player, new Vector3(SaveManager.data.XPosition, SaveManager.data.YPosition, 1),
                                            Quaternion.Euler(0, 0, SaveManager.data.ZRotation));

        if (LevelLoaded != null)
            LevelLoaded();
    }

    void DestroyMultipleInstances()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetPlayer()
    {
        return instantiatedPlayer;
    }
}
