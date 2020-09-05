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
        float dist;
        if (trigger == null) {
            if (Global.Instance().Data.GetSwitch("chaser_spawning")) {
                var chaserX = Global.Instance().Data.GetVariable("chaser_x");
                var chaserY = Global.Instance().Data.GetVariable("chaser_y");
                var target = new Vector3(chaserX, -chaserY, Global.Instance().Maps.Avatar.transform.position.z);
                dist = (target - transform.position).magnitude;
                dist += (Global.Instance().Maps.ChaserSpawnsAt - Time.time) * 2f;
            } else {
                dist = 1000;
            }
        } else {
            dist = (trigger.transform.position - transform.position).magnitude;
        }
        var dist2 = (Global.Instance().Maps.Avatar.Event.transform.position - transform.position).magnitude;
        if (dist2 > dist && trigger != null) dist = 0.0f;
        var ratio = Mathf.Clamp((dist - minDist) / (maxDist - minDist), 0, 1);
        light.range = origRange * ratio;
    }
}
