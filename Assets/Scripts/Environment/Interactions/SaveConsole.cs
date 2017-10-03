using System.Collections;
using UnityEngine;

public class SaveConsole : Interactable {

    Camera cam;
    CameraController camController;
    SaveScreen saveScreen;
    Vector3 consolePos;

    void Start()
    {
        cam = Camera.main;
        camController = cam.GetComponent<CameraController>();
        saveScreen = GameObject.Find("Canvas").GetComponentInChildren<SaveScreen>(true);
        consolePos = transform.position;
    }

    protected override void Interact()
    {
        StartCoroutine(CameraZoom());
        camController.PauseFollow(true);
    }

    IEnumerator CameraZoom()
    {
        while (cam.orthographicSize > 0.5f)
        {
            yield return new WaitForSeconds(0.0001f);
            Vector3 newPos = Vector3.Lerp(cam.transform.position, consolePos, 0.2f);
            newPos.z = cam.transform.position.z;
            cam.transform.position = newPos;
            cam.orthographicSize -= 0.5f;
            if (cam.orthographicSize < 0.5f)
                cam.orthographicSize = 0.5f;
        }
        saveScreen.Enable(true);
        interactions.EnableDialogue(false);
        saveScreen.SetConsole(this);
    }

    IEnumerator CameraZoomOut()
    {
        while (cam.orthographicSize < 10f)
        {
            yield return new WaitForSeconds(0.0001f);
            cam.orthographicSize += 0.5f;
            if (cam.orthographicSize > 10f)
                cam.orthographicSize = 10f;
        }
        saveScreen.Enable(false);
        camController.PauseFollow(false);
        base.StopInteracting();
    }

    public override void StopInteracting()
    {
        StartCoroutine(CameraZoomOut());
    }
}
