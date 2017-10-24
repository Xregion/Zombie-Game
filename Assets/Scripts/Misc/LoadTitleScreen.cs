using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitleScreen : MonoBehaviour {

	void Awake () {
        SceneManager.LoadScene("title screen");
	}
}
