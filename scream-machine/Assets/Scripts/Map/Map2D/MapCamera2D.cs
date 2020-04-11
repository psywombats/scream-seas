using UnityEngine;

public class MapCamera2D : MapCamera {

    public void LateUpdate() {
        ManualUpdate();
    }

    private Vector3 targetPos;
    public override void ManualUpdate() {
        base.ManualUpdate();

        if (target == null) return;

        var cam = GetCameraComponent();
        // assume the target is moving pixel-perfect (w/e)
        targetPos = new Vector3(target.transform.position.x, target.transform.position.y, cam.transform.position.z);
        cam.transform.position = new Vector3(
            targetPos.x + Map.UnitsPerTile / 2.0f * OrthoDir.East.Px2DX(),
            targetPos.y + Map.UnitsPerTile / 2.0f * OrthoDir.North.Px2DY(),
            cam.transform.position.z);
    }
}
