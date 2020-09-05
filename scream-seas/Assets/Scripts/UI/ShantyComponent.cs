using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ShantyComponent : MonoBehaviour {

    public ExpanderComponent expander;

    [Space]
    public TextAutotyper line1;
    public TextAutotyper line2;
    public TextAutotyper line3;
    public TextAutotyper line4;
    public TextAutotyper line5;
    public TextAutotyper line6;
    public TextAutotyper line7;

    [Space]
    public float delay = 0.5f;

    public IEnumerator StartRoutine() {
        Clear();
        MapOverlayUI.Instance().nvl.Wipe();
        yield return MapOverlayUI.Instance().nvl.backer.HideRoutine();
        yield return expander.ShowRoutine();
        Global.Instance().Audio.PlayBGM("shanty");
        yield return Global.Instance().Audio.AwaitBGMLoad();
        StartCoroutine(ParallelRoutine());
    }

    public IEnumerator PlayRoutine(string[] lines) {
        Clear();
        yield return TypeRoutine(line1, lines[0]);
        yield return TypeRoutine(line2, lines[1]);
        yield return TypeRoutine(line3, lines[2]);
        yield return TypeRoutine(line4, lines[3]);
        yield return TypeRoutine(line5, lines[4]);
        yield return TypeRoutine(line6, lines[5]);
        yield return TypeRoutine(line7, lines[6]);
        yield return CoUtils.Wait(delay);
    }

    public IEnumerator FinishRoutine() {
        Clear();
        yield return expander.HideRoutine();
        yield return MapOverlayUI.Instance().nvl.backer.ShowRoutine();
    }

    private IEnumerator TypeRoutine(TextAutotyper typer, string line) {
        var time = Time.time;
        yield return typer.TypeRoutine(line, false);
        while (Time.time < time + delay) {
            yield return null;
        }
    }

    private IEnumerator ParallelRoutine() {
        CharaEvent.disableStep = true;
        var anchor = FindObjectOfType<ShantyMapAnchorComponent>();
        anchor.globalEnable.SetActive(true);
        var time = 30f;
        var nvl = MapOverlayUI.Instance().nvl;
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(nvl.background.DOFade(0.0f, time)),
            CoUtils.RunTween(anchor.panOcean.transform.DOLocalMoveY(-3, time)),
            CoUtils.RunTween(anchor.panMoon.transform.DOLocalMoveY(-1.5f, time)),
        }, this);
        yield return CoUtils.Wait(17);
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(anchor.panShip.transform.DOLocalMoveY(0, time)),
            CoUtils.RunTween(anchor.panOcean.transform.DOLocalMoveY(0, time)),
            CoUtils.RunTween(anchor.panMoon.transform.DOLocalMoveY(0, time)),
        }, this);
        yield return nvl.HideRoutine();
        CharaEvent.disableStep = false;
    }

    private void Clear() {
        line1.Clear();
        line2.Clear();
        line3.Clear();
        line4.Clear();
        line5.Clear();
        line6.Clear();
        line7.Clear();
    }
}
