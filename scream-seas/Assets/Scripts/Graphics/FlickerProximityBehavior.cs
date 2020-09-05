using UnityEngine;
using System.Collections;

public class FlickerProximityBehavior : MonoBehaviour {

    private new Light light;
    private float origIntensity;
    
    public float minDist, maxDist;

    public void Start() {
        light = GetComponent<Light>();
        origIntensity = light.intensity;
    }

    public void Update() {
        var trigger = Global.Instance().Maps.Chaser;
        var dist = 1000f;
        if (trigger != null) {
            dist = (trigger.transform.position - transform.position).magnitude;
        } else if (Global.Instance().Data.GetSwitch("chaser_spawning")) {
            var chaserX = Global.Instance().Data.GetVariable("chaser_x");
            var chaserY = Global.Instance().Data.GetVariable("chaser_y");
            var target = new Vector3(chaserX, -chaserY, Global.Instance().Maps.Avatar.transform.position.z);
            dist = (target - transform.position).magnitude;
        } else {
            return;
        }
       
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
