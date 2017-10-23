using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SpawnPointManager : MonoBehaviour {

    int id;
    bool isDead;
    BasicZombieMain zombie;

    static List<int> ids = new List<int>();

    void Start()
    {
        if (!isDead)
        {
            zombie = GetComponentInChildren<BasicZombieMain>();
            zombie.Died += ZombieDied;
        }
    }

    void OnDisable()
    {
        if (!isDead)
            zombie.Died -= ZombieDied;
    }

    void ZombieDied()
    {
        isDead = true;
        if (SaveManager.data.ZombieSpawnPoints.ContainsKey(id))
            SaveManager.data.ZombieSpawnPoints[id] = isDead;
        else
            SaveManager.data.ZombieSpawnPoints.Add(id, true);
    }

    public void SetIsDead()
    {
        while (ids.Contains(id))
            id++;

        ids.Add(id);

        if (SaveManager.data.ZombieSpawnPoints.ContainsKey(id))
            isDead = SaveManager.data.ZombieSpawnPoints[id];
        else
            isDead = false;
    }

    public List<int> GetIDList()
    {
        return ids;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public int GetID()
    {
        return id;
    }
}
