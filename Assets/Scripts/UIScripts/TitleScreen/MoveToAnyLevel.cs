using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MoveToAnyLevel : MonoBehaviour {

    public Button sceneButton;

    string[] sceneNames;
    int sceneCount;
    Vector2 position;

	void Start () {
        position = new Vector2(-300, 200);
        sceneCount = SceneManager.sceneCountInBuildSettings;
        sceneNames = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            sceneNames[i] = SceneUtility.GetScenePathByBuildIndex(i);

            string sceneName = sceneNames[i].Split('/')[2].Split('.')[0];

            Button button = Instantiate(sceneButton, transform, false);
            button.GetComponentInChildren<Text>().text = sceneName;
            button.GetComponent<RectTransform>().anchoredPosition = position;
            button.onClick.AddListener(delegate { ChangeScenes(sceneName); });

            position -= new Vector2(0, 50);

            if (position.y < -200)
                position.Set(position.x + 200, 200);
        }
	}

    void ChangeScenes(string sceneName)
    {
        if (SaveManager.data.LoadData(1))
            SceneManager.LoadScene(sceneName);
    }
}
