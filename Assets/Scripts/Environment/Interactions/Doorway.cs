using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : Interactable
{
    public string sceneToLoad;
    public Vector3 playerPos;
    public float playerRot;

    protected override void Interact()
    {
        SaveManager.data.XPosition = playerPos.x;
        SaveManager.data.YPosition = playerPos.y;
        SaveManager.data.ZPosition = playerPos.z;
        SaveManager.data.ZRotation = playerRot;
        SceneManager.LoadScene(sceneToLoad);
    }
}
