using UnityEngine;

public class EndDemo : MonoBehaviour {

    public GameObject endScreen;

    TentacleBoss boss;

	void Start () {
        boss = FindObjectOfType<TentacleBoss>();
        boss.DeathEvent += BossKilled;
	}

    void BossKilled()
    {
        PauseScreen ps = FindObjectOfType<PauseScreen>();
        ps.SendOutPauseEvent();
        ps.EnablePause(false);
        endScreen.SetActive(true);
        boss.DeathEvent -= BossKilled;
    }

    public void EndTheDemo()
    {
        Application.OpenURL("http://kylebperri.com/portfolio/zombiegameshow/");
    }
}
