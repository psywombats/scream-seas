using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FlashlightComponent : MonoBehaviour {

    [SerializeField] private CharaEvent chara = null;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private new Light light = null;
    [Space]
    [SerializeField] private Light rightLight = null;
    [SerializeField] private Light upLight = null;
    [SerializeField] private Light leftLight = null;
    [SerializeField] private Light downLight = null;

    private OrthoDir oldFacing = OrthoDir.West;
    private Tweener translateTween;
    private Tweener angleTween;
    private Tweener intensityTween;
    private Tweener spotTween;

    public void Update() {
        var facing = chara.Facing;
        if (facing == oldFacing) {
            return;
        }
        UpdateLights();
    }

    public void OnEnable() {
        UpdateLights();
    }

    private void UpdateLights() {
        oldFacing = chara.Facing;

        if (translateTween != null && translateTween.active) {
            translateTween.Kill();
            angleTween.Kill();
            intensityTween.Kill();
            spotTween.Kill();
        }

        Light target = null;
        switch (chara.Facing) {
            case OrthoDir.East:
                target = rightLight;
                break;
            case OrthoDir.North:
                target = upLight;
                break;
            case OrthoDir.South:
                target = downLight;
                break;
            case OrthoDir.West:
                target = leftLight;
                break;
        }

        translateTween = light.transform.DOLocalMove(target.transform.localPosition, duration);
        angleTween = light.transform.DOLocalRotate(target.transform.localEulerAngles, duration);
        intensityTween = light.DOIntensity(target.intensity, duration);
        spotTween = DOTween.To(() => light.spotAngle, x => light.spotAngle = x, target.spotAngle, duration);

        angleTween.Play();
        translateTween.Play();
        intensityTween.Play();
        spotTween.Play();
    }
}
