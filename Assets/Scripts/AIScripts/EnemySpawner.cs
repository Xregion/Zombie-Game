using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public GameObject prefab;
    public int numberToSpawn;
    public float timer;
    public float maxObjectsAllowedInPlay;

    [HideInInspector]
    public List<GameObject> activeObjects;

    List<GameObject> objects;

    void Start()
    {
        objects = new List<GameObject>();
        activeObjects = new List<GameObject>();

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objects.Add(obj);
        }

        InvokeRepeating("Spawn", 0.5f, timer);
    }

    void Spawn()
    {
        if (activeObjects.Count <= maxObjectsAllowedInPlay)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].activeInHierarchy)
                {
                    AIController ai = objects[i].GetComponent<AIController>();
                    if (!ai.isAlive)
                        ai.ReviveZombie();
                    objects[i].transform.position = transform.position;
                    objects[i].transform.rotation = transform.rotation;
                    objects[i].SetActive(true);
                    activeObjects.Add(objects[i]);
                    break;
                }
            }
        }
    }
}
