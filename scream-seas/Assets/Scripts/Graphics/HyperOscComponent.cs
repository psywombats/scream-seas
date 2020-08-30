using UnityEngine;
using System.Collections;

public class HyperOscComponent : MonoBehaviour {

    public LightOscillator lit;

    public void Update() {
        if (Global.Instance().Data.GetSwitch("finale_mode")) {
            lit.durationSeconds = 0.15f;
        }
    }
}
