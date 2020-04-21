using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class VcrSystem : MonoBehaviour {

    public CanvasGroup canvas;
    public VideoPlayer player;

    public IEnumerator PlayRoutine() {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 0.0f;
        yield return CoUtils.RunTween(canvas.DOFade(1.0f, 0.8f));
        player.Play();
    }

    public IEnumerator HideRoutine() {
        player.Stop();
        yield return CoUtils.RunTween(canvas.DOFade(0.0f, 0.8f));
        canvas.gameObject.SetActive(false);
    }
}
