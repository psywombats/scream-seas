using UnityEngine;

/**
 * Renders the attached sprite as fixed-x at the camera.
 */
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BillboardingSpriteComponent : MonoBehaviour {

    public bool billboardX = true;
    public bool billboardY;

    public void Update() {
        if (GetCamera() == null) {
            return;
        }
        if (billboardX) {
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(
                    GetCamera().transform.eulerAngles.x,
                    angles.y,
                    angles.z);
        }
        if (billboardY) {
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(
                    angles.x,
                    GetCamera().transform.eulerAngles.y,
                    angles.z);
        }
    }

    private Camera GetCamera() {
        if (Application.isPlaying) {
            return GetComponentInParent<MapEvent>().parent.camera;
        } else {
            Camera cam = GetComponentInParent<MapEvent>().parent.camera;
            if (cam != null) return cam;
            return FindObjectOfType<Camera>();
        }
    }
}
