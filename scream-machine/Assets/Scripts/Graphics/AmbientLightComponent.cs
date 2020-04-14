using UnityEngine;
using System.Collections;

public class AmbientLightComponent : MonoBehaviour {

    public bool resets;
    private float oldLight;
    public float ambientLight;

    public void OnEnable() {
        oldLight = RenderSettings.ambientIntensity;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.ambientIntensity = ambientLight;
    }

    public void OnDisable() {
        if (resets) {
            ambientLight = oldLight;
        }
    }
}
