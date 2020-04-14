using UnityEngine;
using System.Collections;

public class ColorLightComponent : MonoBehaviour {
    
    public Color color;
    public float intensity;

    public void OnEnable() {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientEquatorColor = color;
        RenderSettings.ambientGroundColor = color;
        RenderSettings.ambientSkyColor = color;
        RenderSettings.ambientIntensity = intensity;
    }
}
