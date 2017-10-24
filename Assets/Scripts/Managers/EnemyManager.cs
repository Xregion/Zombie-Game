using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    Dictionary<SerializableVector3, bool> spawnPointsDictionary = new Dictionary<SerializableVector3, bool>();

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
                SerializableVector3 id = point.GetID();
                if (!SaveManager.data.ZombieSpawnPoints.ContainsKey(id))
                    SaveManager.data.ZombieSpawnPoints.Add(id, point.GetIsDead());

                spawnPointsDictionary.Add(id, SaveManager.data.ZombieSpawnPoints[id]);
            }
        }

        return spawnPoints;
    }

    public Dictionary<SerializableVector3, bool> GetSpawnPointsDictionary()
    {
        return spawnPointsDictionary;
    }
}
