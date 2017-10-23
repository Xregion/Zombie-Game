using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : Interactable
{
    public string sceneToLoad;
    public Vector2 playerPos;
    public float playerRot;

    protected override void Interact()
    {
        SaveManager.data.XPosition = playerPos.x;
        SaveManager.data.YPosition = playerPos.y;
        SaveManager.data.ZRotation = playerRot;
        if (sceneToLoad != "")
            SceneManager.LoadScene(sceneToLoad);
    }
}
