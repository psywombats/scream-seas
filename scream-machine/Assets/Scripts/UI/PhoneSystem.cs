using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PhoneSystem : MonoBehaviour {

    [SerializeField] private GameObject bigPhone = null;
    [SerializeField] private GameObject miniPhone = null;
    [Space]
    [SerializeField] private float miniPhoneHeight = 250;
    [SerializeField] private float miniPhoneTranslate = 640;
    [SerializeField] private float bigPhoneHeight = 700;
    [Space]
    [SerializeField] private float flipMiniDuration = 0.3f;
    [SerializeField] private float flipBigDuration = 0.6f;

    public bool IsFlipped { get; private set; }

    public IEnumerator FlipRoutine() {
        if (IsFlipped) {
            yield return FlipShutRoutine();
        } else {
            yield return FlipOpenRoutine();
        }
        IsFlipped = !IsFlipped;
    }

    public void Reset() {
        IsFlipped = false;
        bigPhone.GetComponent<CanvasGroup>().alpha = 0.0f;
        miniPhone.GetComponent<RectTransform>().anchoredPosition = default;
        bigPhone.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -bigPhoneHeight);
    }

    private IEnumerator FlipOpenRoutine() {
        StartCoroutine(HideMiniRoutine());
        yield return CoUtils.Wait(flipMiniDuration * .7f);
        StartCoroutine(ShowBigRoutine());
    }

    private IEnumerator FlipShutRoutine() {
        StartCoroutine(HideBigRoutine());
        yield return CoUtils.Wait(flipBigDuration * .7f);
        StartCoroutine(ShowMiniRoutine());
    }

    private IEnumerator HideMiniRoutine() {
        miniPhone.GetComponent<RectTransform>().anchoredPosition = default;
        var shrinkMiniTween = miniPhone.transform.DOLocalMoveY(-miniPhoneHeight, flipMiniDuration);
        var translateMiniTween = miniPhone.transform.DOLocalMoveX(-miniPhoneTranslate, flipMiniDuration);
        shrinkMiniTween.SetEase(Ease.OutQuad);
        translateMiniTween.SetEase(Ease.InQuad);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(shrinkMiniTween),
            CoUtils.RunTween(translateMiniTween),
        }, this);
    }

    private IEnumerator ShowBigRoutine() {
        var transparencyTween = bigPhone.GetComponent<CanvasGroup>().DOFade(1.0f, flipBigDuration);
        var translateTween = bigPhone.GetComponent<RectTransform>().DOAnchorPosY(0.0f, flipBigDuration);
        transparencyTween.SetEase(Ease.InQuad);
        translateTween.SetEase(Ease.OutQuad);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(transparencyTween),
            CoUtils.RunTween(translateTween),
        }, this);
    }

    private IEnumerator HideBigRoutine() {
        var transparencyTween = bigPhone.GetComponent<CanvasGroup>().DOFade(0.0f, flipBigDuration);
        var translateTween = bigPhone.transform.DOLocalMoveY(-bigPhoneHeight, flipBigDuration);
        transparencyTween.SetEase(Ease.OutQuad);
        translateTween.SetEase(Ease.InQuad);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(transparencyTween),
            CoUtils.RunTween(translateTween),
        }, this);
    }

    private IEnumerator ShowMiniRoutine() {
        var shrinkMiniTween = miniPhone.transform.DOLocalMoveY(0.0f, flipMiniDuration);
        var translateMiniTween = miniPhone.transform.DOLocalMoveX(0.0f, flipMiniDuration);
        shrinkMiniTween.SetEase(Ease.InQuad);
        translateMiniTween.SetEase(Ease.OutQuad);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(shrinkMiniTween),
            CoUtils.RunTween(translateMiniTween),
        }, this);
    }
}
