using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CrypticMessageComponent : MonoBehaviour {

    public CanvasGroup messageBlock;
    public TextAutotyper text;

    public IEnumerator BeginRoutine() {
        text.Clear();
        yield return CoUtils.RunTween(messageBlock.DOFade(1, 1.0f));
    }

    public IEnumerator TypeRoutine(string msg) {
        text.Clear();
        yield return text.TypeRoutine(msg, false);
    }

    public IEnumerator EndRoutine() {
        yield return CoUtils.RunTween(messageBlock.DOFade(0, 0.5f));
    }
}
