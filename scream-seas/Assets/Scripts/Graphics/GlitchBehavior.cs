using UnityEngine;
using UnityEngine.UI;

public class GlitchBehavior : MonoBehaviour {
    public bool useWaveSource;

    private Material material;
    private float elapsedSeconds;

    public void Awake() {
        var renderer = GetComponent<Renderer>();
        if (renderer != null) {
            material = renderer.sharedMaterial;
        } else {
            material = GetComponent<Image>().material;
        }
    }

    public void Update() {
        AssignCommonShaderVariables();
        elapsedSeconds += Time.deltaTime;
    }

    private void AssignCommonShaderVariables() {
        material.SetFloat("_Elapsed", elapsedSeconds);
        if (useWaveSource && Global.Instance().Audio.BGMClip() != null && Global.Instance().Audio.GetWaveSource() != null) {
            var samples = Global.Instance().Audio.GetWaveSource().GetSamples();
            if (samples != null) {
                material.SetFloatArray("_Wave", samples);
                material.SetInt("_WaveSamples", Global.Instance().Audio.GetWaveSource().GetSampleCount());
            }
        }
    }
}
