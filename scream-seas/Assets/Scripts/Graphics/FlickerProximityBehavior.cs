using UnityEngine;
using System.Collections;

public class FlickerProximityBehavior : MonoBehaviour {

    private new Light light;
    private float origIntensity;

    public MapEvent trigger;
    public float minDist, maxDist;

    public void Start() {
        light = GetComponent<Light>();
        origIntensity = light.intensity;
    }

    public void Update() {
        var dist = (trigger.transform.position - transform.position).magnitude;
        var ratio = Mathf.Clamp((dist - minDist) / (maxDist - minDist), 0, 1);
        var roll = Random.Range(0, 100);
        var required = (1.0 - ratio) * 50;
        if (roll < required) {
            light.intensity = 0.2f;
        } else {
            light.intensity = origIntensity;
        }
    }
}
