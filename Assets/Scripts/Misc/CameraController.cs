using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform player;

    Vector3 cameraPos;

	void Start () {
        player = LoadManager.instance.GetPlayer().transform;
        cameraPos = transform.position;
	}
	
	void LateUpdate () {
        cameraPos.x = player.position.x;
        cameraPos.y = player.position.y;
        transform.position = cameraPos;
	}
}
