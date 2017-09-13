using UnityEngine;

public class FanSpin : MonoBehaviour {

    Vector3 rotation = new Vector3(0, 0, 0);
    float rotationAngle = 0;

	void Update () {
        rotationAngle -= 20f;
        if (rotationAngle <= -1000)
            rotationAngle = 0;
        rotation.z = rotationAngle;
        transform.rotation = Quaternion.Euler(rotation);
	}
}
