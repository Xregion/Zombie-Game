using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager data;

    string filePath;

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

    int bullets;

    public int Bullets
    {
        get
        {
            return bullets;
        }

        set
        {
            bullets = value;
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

    float zPosition;

    public float ZPosition
    {
        get
        {
            return zPosition;
        }

        set
        {
            zPosition = value;
        }
    }

    float xRotation;

    public float XRotation
    {
        get
        {
            return xRotation;
        }

        set
        {
            xRotation = value;
        }
    }

    float yRotation;

    public float YRotation
    {
        get
        {
            return yRotation;
        }

        set
        {
            yRotation = value;
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

    List<GameObject> items;

    public List<GameObject> Items
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

        filePath = Application.persistentDataPath + "/gameData.dat";

        LoadData();
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        Data data = new Data(scene, bullets, xPosition, yPosition, zPosition, xRotation, yRotation, zRotation, items, isPowerOn);

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Data data = (Data)bf.Deserialize(file);

            scene = data.currentScene;
            bullets = data.bulletsLeft;
            xPosition = data.xPos;
            yPosition = data.yPos;
            zPosition = data.zPos;
            xRotation = data.xRot;
            yRotation = data.yRot;
            zRotation = data.zRot;
            items = data.currentItems;
            isPowerOn = data.isPowerOn;

            file.Close();
        }
    }

    [Serializable]
    struct Data
    {
        public int currentScene;
        public int bulletsLeft;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xRot;
        public float yRot;
        public float zRot;
        public List<GameObject> currentItems;
        public bool isPowerOn;

        public Data(int scene, int bullets, float xPosition, float yPosition, float zPosition, float xRotation, float yRotation, float zRotation, List<GameObject> items, bool _isPowerOn)
        {
            currentScene = scene;
            bulletsLeft = bullets;
            xPos = xPosition;
            yPos = yPosition;
            zPos = zPosition;
            xRot = xRotation;
            yRot = yRotation;
            zRot = zRotation;
            currentItems = items;
            isPowerOn = _isPowerOn;
        }
    }
}
