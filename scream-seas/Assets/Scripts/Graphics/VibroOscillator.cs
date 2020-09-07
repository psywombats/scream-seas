using UnityEngine;
using System.Collections;

public class VibroOscillator : Oscillator {

    public float mod = 0.0f;
    public float modEffect = 0.25f;

    public override void Update() {
        base.Update();

        mod += (Time.deltaTime * modEffect * Random.Range(0, 2) == 0 ? 1f : -1f) / 10f;

        if (mod > 1f) mod = 1f;
        if (mod < -1f) mod = -1f;
    }

    protected override float CalcVectorMult() {
        var orig = base.CalcVectorMult();
        return orig + mod;
    }
}
