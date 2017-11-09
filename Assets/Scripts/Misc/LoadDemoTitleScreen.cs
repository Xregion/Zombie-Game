using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDemoTitleScreen : MonoBehaviour {

	void Awake () {
        SceneManager.LoadScene("demo title screen");
	}
}
