using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverUIComponent : MonoBehaviour {

    public CanvasGroup blocker;

    public IEnumerator GameOverRoutine() {
        yield return CoUtils.RunTween(blocker.DOFade(1.0f, 0.2f));
        yield return CoUtils.Wait(4f);
    }

    public IEnumerator ResumeNormalRoutine() {
        yield return CoUtils.RunTween(blocker.DOFade(0.0f, 3.0f));
        Global.Instance().Maps.Avatar.UnpauseInput();
    }
}
