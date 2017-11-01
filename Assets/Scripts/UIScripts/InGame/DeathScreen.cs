using UnityEngine;

public class DeathScreen : MonoBehaviour {

    GameObject player;
    GameObject panel;

    void OnEnable()
    {
        LoadManager.instance.LevelLoaded += LoadComplete;
    }

    void OnDisable()
    {
        LoadManager.instance.LevelLoaded -= LoadComplete;
    }

    void LoadComplete()
    {
        player = LoadManager.instance.GetPlayer();
    }

    void Start()
    {
        if (player == null)
            player = LoadManager.instance.GetPlayer();

        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
        player.GetComponent<PlayerController>().DeathEvent += PlayerDied;
    }

    void PlayerDied()
    {
        panel.SetActive(true);
    }
}
