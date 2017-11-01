using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : Interactable
{
    public string sceneToLoad;
    public Vector2 playerPos;
    public float playerRot;

    LockedDoor door;

    protected override void Interact()
    {
        door = GetComponent<LockedDoor>();
        SaveManager.data.XPosition = playerPos.x;
        SaveManager.data.YPosition = playerPos.y;
        SaveManager.data.ZRotation = playerRot;
        if (sceneToLoad != "")
            SceneManager.LoadScene(sceneToLoad);

        if (door != null)
        {
            if (door.isOneWayDoor)
            {
                enabled = false;
                door.enabled = true;
            }
        }
    }
}
