using UnityEngine;
using UnityEngine.UI;

public class RemainingEnemies : MonoBehaviour {

    const string beginningOfText = "Enemies Remaining: ";
    Text enemiesRemainingText;

	void Start () {
        enemiesRemainingText = GetComponent<Text>();
	}

    void Update()
    {
        enemiesRemainingText.text = beginningOfText + EnemySpawner.GetEnemiesRemaining();
    }
}
