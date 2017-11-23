using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : Interactable
{
    public string sceneToLoad;
    public Vector2 playerPos;
    public float playerRot;

    LockedDoor lockedDoor;

    protected override void Interact()
    {
        lockedDoor = GetComponent<LockedDoor>();
        SaveManager.data.XPosition = playerPos.x;
        SaveManager.data.YPosition = playerPos.y;
        SaveManager.data.ZRotation = playerRot;
        if (sceneToLoad != "")
            SceneManager.LoadScene(sceneToLoad);

        if (lockedDoor != null)
        {
            if (lockedDoor.isOneWayDoor)
            {
                enabled = false;
                lockedDoor.enabled = true;
            }
        }
    }
}
