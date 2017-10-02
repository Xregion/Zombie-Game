using UnityEngine;

public class LoadManager : MonoBehaviour {

    public static LoadManager instance;


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

        instantiatedPlayer = Instantiate(player, new Vector3(SaveManager.data.XPosition, SaveManager.data.YPosition, SaveManager.data.ZPosition),
    Quaternion.Euler(SaveManager.data.XRotation, SaveManager.data.YRotation, SaveManager.data.ZRotation));
    }

    public GameObject GetPlayer()
    {
        return instantiatedPlayer;
    }
}
