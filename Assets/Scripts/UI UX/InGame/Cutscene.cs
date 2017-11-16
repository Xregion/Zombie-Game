using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour {

    public string nextScene;
    public int panSpeed;
    public List<Vector3> frames;
    public int frameToDelete;
    public int insertFrameAtPosition;

    Camera cam;
    int currentFrame = 0;
    bool canPan;

    void Start ()
    {
        canPan = true;
        cam = Camera.main;
        cam.transform.position = frames[0];
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && canPan)
        {
            if (currentFrame < frames.Count - 1)
                StartCoroutine(PanToNextFrame());
            else
                SceneManager.LoadScene(nextScene);
            currentFrame++;
        }
    }

    IEnumerator PanToNextFrame()
    {
        canPan = false;
        Vector3 nextFrame = frames[currentFrame + 1];
        while (cam.transform.position != nextFrame)
        {
            Vector2 direction = new Vector2(0, 0);
            if (cam.transform.position.x > nextFrame.x)
                direction.x = -1;
            else if (cam.transform.position.x < nextFrame.x)
                direction.x = 1;
            if (cam.transform.position.y > nextFrame.y)
                direction.y = -1;
            else if (cam.transform.position.y < nextFrame.y)
                direction.y = 1;

            cam.transform.Translate(direction * panSpeed * Time.deltaTime);
            if (Vector3.Distance(cam.transform.position, nextFrame) < 0.1f)
                cam.transform.position = nextFrame;
            yield return new WaitForEndOfFrame();
        }
        canPan = true;
    }

    public void AddNewFrame(Vector3 position)
    {
        if (frames == null)
            frames = new List<Vector3>();
        frames.Add(position);
        Debug.Log("Frame " + (frames.Count - 1) + " has been created at " + frames[frames.Count - 1]);
    }

    public void DeleteFrame(int frame)
    {
        if (frames.Count > frame && frame >= 0)
        {
            frames.RemoveAt(frame);
            Debug.Log("Frame " + frame + " has been removed");
        }
        else
            Debug.LogError("Frame " + frame + " does not exist");
    }

    public void DeleteAllFrames()
    {
        frames.Clear();
        Debug.Log("Frames have been cleared");
    }

    public void ViewAllFrames()
    {
        if (frames.Count > 0)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                Debug.Log("Frame " + i + " at " + frames[i]);
            }
        }
        else
            Debug.Log("No frames currently saved");
    }

    public void InsertFrameAtPosition(Vector3 position)
    {
        if (insertFrameAtPosition > frames.Count || insertFrameAtPosition < 0)
            Debug.Log("Only insert between 0 and " + frames.Count);
        else
        {
            frames.Insert(insertFrameAtPosition, position);
            Debug.Log("Frame " + (insertFrameAtPosition) + " has been created at " + frames[insertFrameAtPosition]);
        }
    }
}
