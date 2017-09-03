using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveManager : MonoBehaviour {

    public Text waveText;
    public int numberOfEnemies;
    public float preparationTime;

    int waveNumber = 1;

	void Start () {
        waveText.text = "Wave " + waveNumber;
        EnemySpawner.WaveCleared += NextWave;
        EnemySpawner.SetMaxEnemiesThisWave(numberOfEnemies);
	}
	
	void NextWave()
    {
        waveNumber++;
        numberOfEnemies += 5;
        EnemySpawner.SetMaxEnemiesThisWave(numberOfEnemies);
        waveText.text = "Wave " + waveNumber;
    }

    IEnumerator CountdownUntilNextWave ()
    {
        yield return new WaitForSeconds(preparationTime);

        //EnemySpawner.StartNextWave();
    }
}
