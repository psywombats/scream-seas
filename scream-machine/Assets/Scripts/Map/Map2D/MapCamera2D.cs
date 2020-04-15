﻿using UnityEngine;

public class MapCamera2D : MapCamera {

    public float minX, maxX;
    public float offsetY = 1;
    public GameObject panorama;
    public float panGive = 3.1f;

    public void LateUpdate() {
        ManualUpdate();
    }

    private Vector3 targetPos;
    public override void ManualUpdate() {
        base.ManualUpdate();

        var cam = GetCameraComponent();
        // assume the target is moving pixel-perfect (w/e)
        if (target == null) return;
        targetPos = new Vector3(target.transform.position.x, target.transform.position.y, cam.transform.position.z);
        float x = targetPos.x + Map.UnitsPerTile / 2.0f * OrthoDir.East.Px2DX();
        float y = targetPos.y + Map.UnitsPerTile / 2.0f * OrthoDir.North.Px2DY() + offsetY;

        if (minX != 0 && x < minX) x = minX;
        if (maxX != 0 && x > maxX) x = maxX;

        cam.transform.position = new Vector3(
            x,
            y,
            cam.transform.position.z);

        if (panorama != null) {
            var t = (x - minX) / (maxX - minX);
            panorama.transform.localPosition = new Vector3(panGive * -t, panorama.transform.localPosition.y, panorama.transform.localPosition.z);
        }
    }
}