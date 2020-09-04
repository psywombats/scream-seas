using UnityEngine;

public class MapCamera2D : MapCamera {

    public float minX, maxX;
    public float minY, maxY;
    public float offsetY = 1;
    public GameObject panorama;
    public float panGive = 3.1f, panGiveY;
    public Camera cam;

    public void LateUpdate() {
        ManualUpdate();
    }

    private Vector3 targetPos;
    public override void ManualUpdate() {
        base.ManualUpdate();

        if (target == null) {
            target = Global.Instance().Maps.Avatar?.Event;
        }

        var cam = GetCameraComponent();
        // assume the target is moving pixel-perfect (w/e)
        if (target == null) return;
        targetPos = new Vector3(target.transform.position.x, target.transform.position.y, cam.transform.position.z);
        float x = targetPos.x + Map.UnitsPerTile / 2.0f * OrthoDir.East.Px2DX();
        float y = targetPos.y + Map.UnitsPerTile / 2.0f * OrthoDir.North.Px2DY() + offsetY;

        if (minX != 0 && x < minX) x = minX;
        if (maxX != 0 && x > maxX) x = maxX;

        transform.position = new Vector3(
            x,
            y,
            transform.position.z);

        if (panorama != null) {
            var t = (x - minX) / (maxX - minX);
            var t2 = 0.0f;
            if (minY != 0 && maxY != 0) {
                t2 = (y - minY) / (maxY - minY);
            }
            panorama.transform.localPosition = new Vector3(panGive * -t, t2 != 0 ? panGiveY * t2 : panorama.transform.localPosition.y, panorama.transform.localPosition.z);
        }
    }

    public override Camera GetCameraComponent() {
        return cam;
    }
}
