using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class LoadManager : MonoBehaviour {

    public static LoadManager instance;

    public event Action LevelLoaded;

    public GameObject playerPrefab;
    public GameObject zombiePrefab;
    public List<string> outOfGameScenes;

    GameObject instantiatedPlayer;
    List<int> idsAtScene;

    void Awake()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    private void SceneChange(Scene previousScene, Scene loadedScene)
    {
        DestroyMultipleInstances();
        if (!outOfGameScenes.Contains(loadedScene.name))
        {
            SpawnPlayer();
            if (loadedScene != SceneManager.GetSceneByName("main"))
                SpawnZombies();
        }

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

    void SpawnPlayer ()
    {
        instantiatedPlayer = Instantiate(playerPrefab, new Vector3(SaveManager.data.XPosition, SaveManager.data.YPosition, 1),
                                            Quaternion.Euler(0, 0, SaveManager.data.ZRotation));
    }

    void SpawnZombies ()
    {
        EnemyManager enemyManager = GetComponent<EnemyManager>();
        if (enemyManager != null)
        {
            GameObject[] points = enemyManager.FindSpawnPoints();
            if (points != null)
            {
                Dictionary<SerializableVector3, bool> enemyDictionary = enemyManager.GetSpawnPointsDictionary();

                int i = 0;
                foreach (SerializableVector3 id in enemyDictionary.Keys)
                {
                    if (points[i].transform.position != id)
                        continue;
                    if (!enemyDictionary[id])
                    {
                        Instantiate(zombiePrefab, points[i].transform.position, points[i].transform.rotation, points[i].transform);
                    }
                    i++;
                }
            }
        }
    }
}
