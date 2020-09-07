using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class ShantyComponent : MonoBehaviour {

    public ExpanderComponent expander;
    public Image backer;

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

    public IEnumerator StartRoutine(bool finale = false) {
        Clear();
        if (finale) {
            backer.color = new Color(0, 0, 0, 0);
        }
        yield return expander.ShowRoutine();
        Global.Instance().Audio.PlayBGM("shanty");
        yield return Global.Instance().Audio.AwaitBGMLoad();
        StartCoroutine(ParallelRoutine(finale));
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

    private IEnumerator ParallelRoutine(bool finale) {
        if (!finale) {
            CharaEvent.disableStep = true;
            var anchor = FindObjectOfType<ShantyMapAnchorComponent>();
            anchor.globalEnable.SetActive(true);
            var time = 30f;
            yield return CoUtils.RunParallel(new IEnumerator[] {
                    CoUtils.RunTween(anchor.panOcean.transform.DOLocalMoveY(-3, time)),
                    CoUtils.RunTween(anchor.panMoon.transform.DOLocalMoveY(-1.5f, time)),
            }, this);
            yield return CoUtils.Wait(17);
            yield return CoUtils.RunParallel(new IEnumerator[] {
                    CoUtils.RunTween(anchor.panShip.transform.DOLocalMoveY(0, time)),
                    CoUtils.RunTween(anchor.panOcean.transform.DOLocalMoveY(0, time)),
                    CoUtils.RunTween(anchor.panMoon.transform.DOLocalMoveY(0, time)),
            }, this);
            CharaEvent.disableStep = false;
        } else {
            CharaEvent.disableStep = true;
            Global.Instance().Maps.Camera.track = false;
            var anchor = FindObjectOfType<ShantyMapAnchorComponent>();
            anchor.globalEnable.SetActive(true);
            var timex = 78f;
            var twn = anchor.panOcean.transform.DOLocalMoveY(2.2f, timex);
            twn.SetEase(Ease.Linear);
            yield return CoUtils.RunParallel(new IEnumerator[] {
                    CoUtils.RunTween(anchor.panShip.transform.DOLocalMoveY(-11, timex)),
                    CoUtils.RunTween(twn),
            }, this);
            var time2 = 17f;
            Global.Instance().Audio.PlayBGM("redsky");
            Global.Instance().Data.SetSwitch("finale_boat", true);
            anchor.finaleParticle.gameObject.SetActive(false);
            yield return CoUtils.RunParallel(new IEnumerator[] {
                    CoUtils.RunTween(anchor.finaleCloud1.DOColor(new Color(0, 0, 0, 0), time2)),
                    CoUtils.RunTween(anchor.finaleCould2.DOColor(new Color(0, 0, 0, 0), time2)),
                    CoUtils.RunTween(anchor.finaleMoon.DOColor(new Color(1, 1, 1, 0), time2)),
                    CoUtils.RunTween(anchor.finaleBoat.DOColor(new Color(1, 1, 1, 1), 0.2f)),
                    CoUtils.RunTween(anchor.finaleUnderlay.DOColor(new Color(1, 1, 1, 1), time2)),
                    CoUtils.RunTween(anchor.panOcean.transform.DOLocalMoveY(4.1f, time2 *2f / 3f)),
            }, this);
            Global.Instance().Audio.PlaySFX("foghorn");
            CharaEvent.disableStep = false;
        }
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
