using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cutscene))]
public class CutsceneCreator : Editor
{

    Camera cam;

    void OnEnable()
    {
        cam = Camera.main;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Cutscene cutscene = (Cutscene)target;

        if (GUILayout.Button("Create Next Frame at Camera Position"))
        {
            cutscene.AddNewFrame(cam.transform.position);
        }
        if (GUILayout.Button("Delete Current Frame"))
        {
            cutscene.DeleteFrame(cutscene.frameToDelete);
        }
        if (GUILayout.Button("Delete All Frames"))
        {
            cutscene.DeleteAllFrames();
        }
        if (GUILayout.Button("View All Frames"))
        {
            cutscene.ViewAllFrames();
        }
        if (GUILayout.Button("Insert Frame at Position"))
        {
            cutscene.InsertFrameAtPosition(cam.transform.position);
        }
    }
}
