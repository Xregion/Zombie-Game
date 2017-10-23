using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    Dictionary<int, bool> spawnPointsDictionary = new Dictionary<int, bool>();

    public GameObject[] FindSpawnPoints()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPointsDictionary.Clear();

        if (spawnPoints != null)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                SpawnPointManager point = spawnPoints[i].GetComponent<SpawnPointManager>();
                point.SetIsDead();
                int id = point.GetID();
                if (!SaveManager.data.ZombieSpawnPoints.ContainsKey(id))
                    SaveManager.data.ZombieSpawnPoints.Add(id, point.GetIsDead());

                spawnPointsDictionary.Add(id, SaveManager.data.ZombieSpawnPoints[id]);
                print(id + " " + SaveManager.data.ZombieSpawnPoints[id]);
            }
        }

        return spawnPoints;
    }

    public Dictionary<int, bool> GetSpawnPointsDictionary()
    {
        return spawnPointsDictionary;
    }
}
