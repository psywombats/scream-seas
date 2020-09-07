using UnityEngine;
using System.Collections;

public class LoopingVelocityComponent : MonoBehaviour {

    public float startX = -1;
    public Vector3 velocity = new Vector3(1, 0, 0);
    public float repeatX = 1;

    public void Update() {
        transform.localPosition += velocity * Time.deltaTime;
        if (velocity.x > 0 && transform.localPosition.x > repeatX ||
                    velocity.x < 0 && transform.localPosition.x < repeatX ) {
            transform.localPosition = new Vector3(
                startX,
                transform.localPosition.y,
                transform.localPosition.z);
        }
    }
}
