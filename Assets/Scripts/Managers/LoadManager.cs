using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadManager : MonoBehaviour {

    public static LoadManager instance;

    public event Action LevelLoaded;

    public GameObject player;
    GameObject instantiatedPlayer;

    void Awake()
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

        SceneManager.activeSceneChanged += SceneChange;
    }

    private void SceneChange(Scene arg0, Scene arg1)
    {
        if (arg1 != SceneManager.GetSceneByName("title screen"))
            instantiatedPlayer = Instantiate(player, new Vector3(SaveManager.data.XPosition, SaveManager.data.YPosition, SaveManager.data.ZPosition),
                                            Quaternion.Euler(0, 0, SaveManager.data.ZRotation));   

        if (LevelLoaded != null)
            LevelLoaded();
    }

    void Start()
    {
        if (LevelLoaded != null)
            LevelLoaded();
    }

    public GameObject GetPlayer()
    {
        return instantiatedPlayer;
    }
}
