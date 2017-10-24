using UnityEngine;

public class SpawnPointManager : MonoBehaviour {

    SerializableVector3 id;
    bool isDead;
    BasicZombieMain zombie;

    void Start()
    {
        id = transform.position;
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
        id = transform.position;
        if (SaveManager.data.ZombieSpawnPoints.ContainsKey(id))
            isDead = SaveManager.data.ZombieSpawnPoints[id];
        else
            isDead = false;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public Vector3 GetID()
    {
        return id;
    }
}
