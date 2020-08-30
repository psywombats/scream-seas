﻿using UnityEngine;

[RequireComponent(typeof(FadeImageEffect))]
public class MapCamera : MonoBehaviour {
    
    public MapEvent target;

    // these are read by sprites, not actually enforced by the cameras
    public bool billboardX;
    public bool billboardY;

    public void OnEnable() {
        Global.Instance().Maps.Camera = this;
        if (target == null && Global.Instance().Maps.Avatar != null) {
            target = Global.Instance().Maps.Avatar.Event;
        }
    }

    public virtual void ManualUpdate() {

    }

    public virtual Camera GetCameraComponent() {
        return GetComponent<Camera>();
    }
}
