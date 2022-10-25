﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightOscillator : Oscillator {

    public Color newColor = Color.white;
    public float rangeOffset;
    public float intensityOffset;

    [Space]
    [Range(0, 0.5f)] public float flickerChance;
    public float flickerRange;
    public float flickerIntensity;
    public Color flickerColor = Color.white;

    private Color originalColor;
    private float originalIntensity;
    private float originalRange;

    public override void Start() {
        base.Start();

        Light light = GetComponent<Light>();
        originalColor = light.color;
        originalIntensity = light.intensity;
        originalRange = light.range;
    }

    public override void Update() {
        if (Global.Instance().Data.GetSwitch("chaser_active") || Global.Instance().Data.GetSwitch("chaser_spawning")) {
            return;
        }

        float vectorMult = CalcVectorMult();
        Light light = GetComponent<Light>();

        if (flickerChance > 0 && Random.Range(0.0f, 1.0f) < flickerChance) {
            light.intensity = originalIntensity + flickerIntensity;
            light.range = originalRange + flickerRange;
            light.color = flickerColor;
        } else {
            light.intensity = originalIntensity + intensityOffset * vectorMult;
            light.range = originalRange + rangeOffset * vectorMult;
            var tempColor = originalColor * (1.0f - vectorMult) + newColor * vectorMult;
            light.color = new Color(
                Mathf.Clamp01(tempColor.r),
                Mathf.Clamp01(tempColor.g),
                Mathf.Clamp01(tempColor.b),
                Mathf.Clamp01(tempColor.a));
        }
    }
}
