using UnityEngine.SceneManagement;

public class Doorway : Interactable
{
    public string sceneToLoad;

    protected override void Interact()
    {
    SceneManager.LoadScene(sceneToLoad);
    }
}
