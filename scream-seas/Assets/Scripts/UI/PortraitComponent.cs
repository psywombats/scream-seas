using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PortraitComponent : MonoBehaviour {

    private static readonly float fadeTime = 0.5f;
    private static readonly float highlightTime = 0.3f;
    private static readonly float inactiveAlpha = 0.5f;

    public Image sprite;
    public bool moveSibling;

    public SpeakerData Speaker { get; private set; }
    public bool IsHighlighted { get; private set; }

    public void Clear() {
        Speaker = null;
        sprite.sprite = null;
        IsHighlighted = false;
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, bool alt = false) {
        if (this.Speaker != null) {
            yield return ExitRoutine();
        }
        this.Speaker = speaker;
        if (alt) {
            sprite.sprite = speaker.altimage;
        } else {
            sprite.sprite = speaker.image;
        }
        
        sprite.SetNativeSize();
        sprite.color = new Color(1, 1, 1, 0);
        IsHighlighted = true;
        var tween = sprite.DOFade(1.0f, fadeTime);
        yield return CoUtils.RunTween(tween);

        IsHighlighted = true;
    }

    public IEnumerator ExitRoutine() {
        if (Speaker != null) {
            var tween = sprite.DOFade(0.0f, fadeTime);
            yield return CoUtils.RunTween(tween);
            Clear();
        }
        if (moveSibling) {
            //transform.SetAsFirstSibling();
        }
    }

    public IEnumerator HighlightRoutine() {
        if (sprite.color.r == 1.0f) {
            yield break;
        }
        var tween = sprite.DOColor(new Color(1, 1, 1, 1), highlightTime);
        if (moveSibling) {
            //transform.SetAsLastSibling();
        }
        yield return CoUtils.RunTween(tween);

        IsHighlighted = true;
    }

    public IEnumerator UnhighlightRoutine() {
        if (Speaker == null || sprite.color.r == inactiveAlpha) {
            yield break;
        }
        var tween = sprite.DOColor(new Color(inactiveAlpha, inactiveAlpha, inactiveAlpha, 1), highlightTime);
        yield return CoUtils.RunTween(tween);
        if (moveSibling) {
            //transform.SetAsFirstSibling();
        }

        IsHighlighted = false;
    }
}
