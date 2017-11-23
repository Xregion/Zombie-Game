using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform player;

    Vector3 cameraPos;
    bool pauseFollow;

    void Start () {
        player = LoadManager.instance.GetPlayer().transform;
        cameraPos = transform.position;
        pauseFollow = false;
	}
	
	void LateUpdate () {
        if (!pauseFollow)
        {
            cameraPos.x = player.position.x;
            cameraPos.y = player.position.y;
            transform.position = cameraPos;
        }
	}

    public void PauseFollow (bool stopFollowing)
    {
        pauseFollow = stopFollowing;
    }
}
