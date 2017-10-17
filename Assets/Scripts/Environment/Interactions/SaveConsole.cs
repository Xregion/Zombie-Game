﻿using System.Collections;
using UnityEngine;

public class SaveConsole : Interactable {

    Camera cam;
    CameraController camController;
    SaveScreen saveScreen;
    Vector3 consolePos;
    float originalCamSize;
    float zoomedCamSize;
    float zoomSpeed;
    bool zooming;

    void Start()
    {
        cam = Camera.main;
        originalCamSize = cam.orthographicSize;
        zoomedCamSize = 0.5f;
        zoomSpeed = 0.1f;
        camController = cam.GetComponent<CameraController>();
        saveScreen = FindObjectOfType<SaveScreen>();
        consolePos = transform.position;
    }

    //TODO need to fix pause event so that player doesn't regain controls when it's called
    protected override void Interact()
    {
        StartCoroutine(CameraZoom());
        camController.PauseFollow(true);
    }

    IEnumerator CameraZoom()
    {
        while (cam.orthographicSize > 0.5f)
        {
            zooming = true;
            yield return new WaitForEndOfFrame();
            Vector3 newPos = Vector3.Lerp(cam.transform.position, consolePos, 0.2f);
            newPos.z = cam.transform.position.z;
            cam.transform.position = newPos;
            cam.orthographicSize -= zoomSpeed;
            if (cam.orthographicSize < zoomedCamSize)
                cam.orthographicSize = zoomedCamSize;
        }
        zooming = false;
        saveScreen.Enable(true);
        interactions.EnableDialogue(false);
        saveScreen.SetConsole(this);
    }

    IEnumerator CameraZoomOut()
    {
        while (cam.orthographicSize < originalCamSize)
        {
            yield return new WaitForEndOfFrame();
            cam.orthographicSize += zoomSpeed;
            if (cam.orthographicSize > originalCamSize)
                cam.orthographicSize = originalCamSize;
        }
        camController.PauseFollow(false);
        base.StopInteracting();
    }

    public override void StopInteracting()
    {
        if (!zooming)
        {
            saveScreen.Enable(false);
            StartCoroutine(CameraZoomOut());
        }
    }
}
