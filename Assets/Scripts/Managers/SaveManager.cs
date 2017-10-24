using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager data;

    string filePath;
    string fileOne;
    string fileTwo;
    string fileThree;

    #region Data to Save/Load
    int scene;

    public int Scene
    {
        get
        {
            return scene;
        }

        set
        {
            scene = value;
        }
    }

    int health;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    int bulletsRemaining;

    public int BulletsRemaining
    {
        get
        {
            return bulletsRemaining;
        }

        set
        {
            bulletsRemaining = value;
        }
    }

    int bulletsInChamber;

    public int BulletsInChamber
    {
        get
        {
            return bulletsInChamber;
        }

        set
        {
            bulletsInChamber = value;
        }
    }

    float xPosition;

    public float XPosition
    {
        get
        {
            return xPosition;
        }

        set
        {
            xPosition = value;
        }
    }

    float yPosition;

    public float YPosition
    {
        get
        {
            return yPosition;
        }

        set
        {
            yPosition = value;
        }
    }

    float zRotation;

    public float ZRotation
    {
        get
        {
            return zRotation;
        }

        set
        {
            zRotation = value;
        }
    }

    List<string> items;

    public List<string> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    bool isPowerOn;

    public bool IsPowerOn
    {
        get
        {
            return isPowerOn;
        }

        set
        {
            isPowerOn = value;
        }
    }

    Dictionary<SerializableVector3, bool> zombieSpawnPoints;

    public Dictionary<SerializableVector3, bool> ZombieSpawnPoints
    {
        get
        {
            return zombieSpawnPoints;
        }

        set
        {
            zombieSpawnPoints = value;
        }
    }
    #endregion

    void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }

        filePath = Application.persistentDataPath;
        fileOne = filePath + "/GameDataOne.zomb";
        fileTwo = filePath + "/GameDataTwo.zomb";
        fileThree = filePath + "/GameDataThree.zomb";
    }

    public void SaveData(int fileNumber)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        switch (fileNumber)
        {
            case 1:
                file = File.Create(fileOne);
                break;
            case 2:
                file = File.Create(fileTwo);
                break;
            case 3:
                file = File.Create(fileThree);
                break;
            default:
                Debug.LogError("File number outside of 1-3");
                return;
        }

        Data data = new Data(scene, health, bulletsRemaining, bulletsInChamber, xPosition, yPosition, zRotation, items, isPowerOn, zombieSpawnPoints);

        bf.Serialize(file, data);
        file.Close();
    }

    // Deserializes the save file from binary and sets the variables based on the data on file.
    // Returns true if the file exists so that anyone accessing the function knows if the file exists or not.
    public bool LoadData(int fileNumber)
    {
        String filePath;
        switch (fileNumber)
        {
            case 1:
                filePath = fileOne;
                break;
            case 2:
                filePath = fileTwo;
                break;
            case 3:
                filePath = fileThree;
                break;
            default:
                Debug.LogError("File number outside of 1-3");
                return false;
        }
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Data data = (Data)bf.Deserialize(file);

            scene = data.currentScene;
            health = data.currentHealth;
            bulletsRemaining = data.bulletsRemaining;
            bulletsInChamber = data.bulletsInChamber;
            xPosition = data.xPos;
            yPosition = data.yPos;
            zRotation = data.zRot;
            items = data.currentItems;
            isPowerOn = data.isPowerOn;
            zombieSpawnPoints = data.zombieSpawnPoints;

            file.Close();
            return true;
        }

        return false;
    }

    [Serializable]
    struct Data
    {
        public int currentScene;
        public int currentHealth;
        public int bulletsRemaining;
        public int bulletsInChamber;
        public float xPos;
        public float yPos;
        public float zRot;
        public List<string> currentItems;
        public bool isPowerOn;
        public Dictionary<SerializableVector3, bool> zombieSpawnPoints;

        public Data(int scene, int health, int bullets, int chamber, float xPosition, float yPosition, float zRotation, List<string> items, bool _isPowerOn, Dictionary<SerializableVector3, bool> spawnPoints)
        {
            currentScene = scene;
            currentHealth = health;
            bulletsRemaining = bullets;
            bulletsInChamber = chamber;
            xPos = xPosition;
            yPos = yPosition;
            zRot = zRotation;
            currentItems = items;
            isPowerOn = _isPowerOn;
            zombieSpawnPoints = spawnPoints;
        }
    }
}
