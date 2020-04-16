using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;

public class PCSystem : MonoBehaviour {

    public enum PCMode { News, Slideshow }

    [SerializeField] private List<CanvasGroup> toFade = null;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private Camera cam = null;
    [Space]
    [SerializeField] private PCMode mode = PCMode.News;
    [Space]
    [SerializeField] private PCNewsView newsView = null;
    [SerializeField] private PCNewsData newsModel = null;
    [Space]
    [SerializeField] private PCSlideshowView slideshowView = null;
    [SerializeField] private PCSlideshowData slideshowModel = null;


    public async Task DoMenuAsync() {
        Global.Instance().Maps.Avatar.PauseInput();

        switch (mode) {
            case PCMode.News:
                newsView.Populate(newsModel);
                break;
            case PCMode.Slideshow:
                slideshowView.Populate(slideshowModel);
                break;
        }

        await FadeRoutine(1.0f);

        switch (mode) {
            case PCMode.News:
                await Global.Instance().Input.ConfirmRoutine();
                break;
            case PCMode.Slideshow:
                await slideshowView.DoSlideshowAsync();
                break;
        }

        await FadeRoutine(0.0f);
        Global.Instance().Maps.Avatar.UnpauseInput();
    }

    public void SetNewsModel(PCNewsData model) {
        SetMode(PCMode.News);
        newsModel = model;
    }

    public void SetSlideshowModel(PCSlideshowData model) {
        SetMode(PCMode.Slideshow);
        slideshowModel = model;
    }

    public void Show() {
        foreach (var canvas in toFade) {
            canvas.alpha = 1.0f;
        }
    }

    public void Hide() {
        foreach (var canvas in toFade) {
            canvas.alpha = 0.0f;
        }
    }

    private void SetMode(PCMode mode) {
        this.mode = mode;
        switch (mode) {
            case PCMode.News:
                newsView.gameObject.SetActive(true);
                slideshowView.gameObject.SetActive(false);
                break;
            case PCMode.Slideshow:
                newsView.gameObject.SetActive(false);
                slideshowView.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator FadeRoutine(float target) {
        if (target == 1.0f) cam.gameObject.SetActive(true);
        var tweens = new List<Tweener>();
        foreach (var canvas in toFade) {
            tweens.Add(canvas.DOFade(target, duration));
        }
        yield return CoUtils.RunParallel(tweens.Select(tween => CoUtils.RunTween(tween)).ToArray(), this);
        if (target == 0.0f) cam.gameObject.SetActive(false);
    }
}
