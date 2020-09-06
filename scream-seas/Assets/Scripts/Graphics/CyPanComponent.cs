using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class CyPanComponent : MonoBehaviour {

    private float time = 8f;

    public GameObject panShip;
    public GameObject panOcean;
    public GameObject panMoon;
    [Space]
    public List<GameObject> turnOn;
    public List<GameObject> turnOnLater;
    public List<GameObject> turnOffLater;
    public List<GameObject> enableStuff;
    public List<GameObject> disableStuff;

    public IEnumerator PanRoutine() {
        Global.Instance().Maps.Camera.track = false;
        CharaEvent.disableStep = true;
        var routines = new IEnumerator[] {
            CoUtils.RunTween(panShip.transform.DOLocalMoveY(-13, time)),
            CoUtils.RunTween(panOcean.transform.DOLocalMoveY(5, time)),
            CoUtils.RunTween(panMoon.transform.DOLocalMoveY(0, time)),
        };
        yield return CoUtils.RunParallel(routines, this);
    }

    public IEnumerator EatRoutine() {
        Global.Instance().Audio.PlaySFX("chaser_howl");

        foreach (var item in turnOn) {
            item.SetActive(true);
        }

        var elapsed = 0.0f;
        var flip = false;
        while (elapsed < 0.7f || flip) {
            flip = !flip;
            elapsed += Time.deltaTime;
            foreach (var item in enableStuff) {
                item.SetActive(flip);
            }
            foreach (var item in disableStuff) {
                item.SetActive(!flip);
            }
            yield return null;
        }

        foreach (var item in turnOnLater) {
            item.SetActive(true);
        }

        foreach (var item in turnOffLater) {
            item.SetActive(false);
        }

        yield return CoUtils.Wait(1.0f);

        var routines = new IEnumerator[] {
            CoUtils.RunTween(panShip.transform.DOLocalMoveY(0, time / 3f)),
            CoUtils.RunTween(panOcean.transform.DOLocalMoveY(0, time / 3f)),
            CoUtils.RunTween(panMoon.transform.DOLocalMoveY(0, time / 3f)),
        };
        yield return CoUtils.RunParallel(routines, this);
        CharaEvent.disableStep = false;
        Global.Instance().Maps.Camera.track = true;
    }
}
