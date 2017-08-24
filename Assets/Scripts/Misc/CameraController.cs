using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;

    Vector3 cameraPos;

	void Start () {
        cameraPos = transform.position;
	}
	
	void LateUpdate () {
        cameraPos.x = player.position.x;
        cameraPos.y = player.position.y;
        transform.position = cameraPos;
	}
}
