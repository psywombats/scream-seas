using UnityEngine;

public class ChaserSpeedComponent : MonoBehaviour {

    private float originalSpeed;

    public void Update() {
        if (Global.Instance().Data.GetSwitch("chaser_active")) {
            Global.Instance().Maps.Avatar.Event.SpeedMult = 1.5f;
        } else {
            Global.Instance().Maps.Avatar.Event.SpeedMult = 1.0f;
        }
    }
}
