using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public static event Action WaveCleared;

    public GameObject prefab;
    public int numberToSpawn;
    public float timer;
    public Transform [] spawnPoints;

    static int numberOfEnemiesLeft;
    static int numberOfEnemiesInPlay;

    List<GameObject> activeObjects;
    List<GameObject> objects;
    static EnemySpawner spawner;

    void Start()
    {
        objects = new List<GameObject>();
        activeObjects = new List<GameObject>();
        spawner = this;

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            obj.GetComponent<AIController>().DeathEvent += RemoveFromList;
            objects.Add(obj);
        }

        InvokeRepeating("Spawn", 0.5f, timer);
    }

    void Spawn()
    {
        if (numberOfEnemiesInPlay >= numberOfEnemiesLeft)
        {
            CancelInvoke();
            return;
        }

        int spawnPosition = UnityEngine.Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].activeInHierarchy)
            {
                AIController ai = objects[i].GetComponent<AIController>();
                if (!ai.isAlive)
                    ai.ReviveZombie();
                objects[i].transform.position = spawnPoints[spawnPosition].position;
                objects[i].transform.rotation = spawnPoints[spawnPosition].rotation;
                objects[i].SetActive(true);
                activeObjects.Add(objects[i]);
                numberOfEnemiesInPlay++;
                break;
            }
        }
    }

    public static void SetMaxEnemiesThisWave (int amount)
    {
        numberOfEnemiesLeft = amount;
    }

    public static int GetEnemiesRemaining ()
    {
        return numberOfEnemiesLeft;
    }

    public static void StartNextWave()
    {
        spawner.InvokeRepeating("Spawn", 0.5f, spawner.timer);
    }

    void RemoveFromList (GameObject go)
    {
        activeObjects.Remove(go);
        numberOfEnemiesInPlay--;
        numberOfEnemiesLeft--;
        if (numberOfEnemiesLeft <= 0)
        {
            CancelInvoke();
            if (WaveCleared != null)
                WaveCleared();
        }
    }
}
