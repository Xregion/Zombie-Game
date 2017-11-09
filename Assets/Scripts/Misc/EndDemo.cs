using UnityEngine;

public class EndDemo : MonoBehaviour {

    public GameObject endScreen;

    GameObject[] enemies;
    int numberOfEnemies;

	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            e.GetComponent<BasicZombieMain>().Died += ZombieDied;
            numberOfEnemies++;
        }
	}

    private void ZombieDied()
    {
        numberOfEnemies--;
        if (numberOfEnemies <= 0)
        {
            PauseScreen ps = FindObjectOfType<PauseScreen>();
            ps.SendOutPauseEvent();
            ps.EnablePause(false);
            endScreen.SetActive(true);
        }
    }

    public void EndTheDemo()
    {
        Application.OpenURL("http://kylebperri.com/portfolio/zombiegameshow/");
    }
}
