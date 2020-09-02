using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class TitleComponent : MonoBehaviour {

    public CanvasGroup titleGroup;
    public Camera cam;
    public SpriteRenderer moon;
    public SpriteRenderer underlay;
    public FadeImageEffect fade;

    [Space]
    public AudioSource currentAudio;
    public AudioSource newAudio;

    [Space]
    public float titleTime = 1.0f;
    public float camY = 2.15f;
    public float camTime = 10f;
    public float moonTime = 3.0f;
    public float delay = 2.0f;

    public void Start() {
        StartCoroutine(TitleRoutine());
    }

    private IEnumerator TitleRoutine() {
        yield return Global.Instance().Input.ConfirmRoutine();
        currentAudio.DOFade(0.0f, camTime).Play();
        newAudio.Play();
        newAudio.DOFade(1.0f, camTime).Play();
        yield return CoUtils.RunTween(titleGroup.DOFade(0.0f, 1.0f));
        cam.transform.DOMoveY(camY, camTime).Play();
        yield return CoUtils.RunTween(moon.DOFade(0.0f, moonTime));
        yield return CoUtils.RunTween(underlay.DOColor(Color.white, camTime - moonTime));
        yield return CoUtils.Wait(delay);
        Global.Instance().Audio.PlaySFX("foghorn");

        StartCoroutine(Global.Instance().Audio.FadeOutRoutine(3.0f));
        TransitionData data = IndexDatabase.Instance().Transitions.GetData("fade_long");
        yield return fade.FadeRoutine(IndexDatabase.Instance().Fades.GetData(data.FadeOutTag));
        yield return CoUtils.Wait(3.0f);
        Global.Instance().StartCoroutine(Global.Instance().Maps.NewGameRoutine(fade));
    }
}
