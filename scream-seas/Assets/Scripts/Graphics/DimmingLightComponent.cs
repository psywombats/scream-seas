using UnityEngine;
using System.Collections;

public class DimmingLightComponent : MonoBehaviour {

    private new Light light;
    private float origRange;
    
    public float minDist, maxDist;

    public void Start() {
        light = GetComponent<Light>();
        origRange = light.range;
    }

    public void Update() {
        var trigger = Global.Instance().Maps.Chaser;
        if (trigger == null) {
            return;
        }
        var dist = (trigger.transform.position - transform.position).magnitude;
        if (!trigger.IsSwitchEnabled) {
            dist = 1000;
        }
        var ratio = Mathf.Clamp((dist - minDist) / (maxDist - minDist), 0, 1);
        light.range = origRange * ratio;
    }
}
