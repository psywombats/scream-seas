using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVLComponent : MonoBehaviour {

    public PortraitComponent slotA;
    public PortraitComponent slotB;
    public PortraitComponent slotC;
    public PortraitComponent slotD;
    public PortraitComponent slotE;

    public ExpanderComponent backer;
    public LineAutotyper text;
    public CanvasGroup fader;

    public void Wipe() {
        text.Clear();
    }

    public IEnumerator ShowRoutine() {
        backer.Hide();
        fader.alpha = 0.0f;
        foreach (var portrait in GetPortraits()) {
            portrait.Clear();
        }
        yield return backer.ShowRoutine();
        text.Clear();
        Wipe();
    }

    public IEnumerator HideRoutine() {
        var routines = new List<IEnumerator>();
        foreach (var portrait in GetPortraits()) {
            if (portrait.Speaker != null) {
                routines.Add(portrait.ExitRoutine());
            }
        }
        routines.Add(backer.HideRoutine());
        routines.Add(CoUtils.RunTween(fader.DOFade(0.0f, backer.duration)));
        yield return CoUtils.RunParallel(routines.ToArray(), this);
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string slot) {
        var portrait = GetPortrait(slot);
        yield return portrait.EnterRoutine(speaker);
    }

    public IEnumerator ExitRoutine(SpeakerData speaker) {
        foreach (var portrait in GetPortraits()) {
            if (portrait.Speaker == speaker) {
                yield return portrait.ExitRoutine();
            }
        }
    }

    public IEnumerator SpeakRoutine(SpeakerData speaker, string message) {
        var portrait = GetPortrait(speaker);
        if (speaker != null) {
            var routines = new List<IEnumerator>();
            if (portrait != null && !portrait.IsHighlighted) {
                routines.Add(portrait.HighlightRoutine());
            }
            foreach (var other in GetPortraits()) {
                if (other.Speaker != null && other.Speaker != speaker && other.IsHighlighted) {
                    routines.Add(other.UnhighlightRoutine());
                }
            }
            yield return CoUtils.RunParallel(routines.ToArray(), this);
        }

        string toType;
        if (portrait != null) {
            toType = "\"" + message + "\"";
        } else {
            toType = message + "";
        }
        yield return text.WriteLineRoutine(toType);
        yield return text.WriteLineRoutine("");
        yield return Global.Instance().Input.ConfirmRoutine();
    }

    private PortraitComponent GetPortrait(string slot) {
        PortraitComponent portrait = null;
        switch (slot.ToLower()) {
            case "a": portrait = slotA; break;
            case "b": portrait = slotB; break;
            case "c": portrait = slotC; break;
            case "d": portrait = slotD; break;
            case "e": portrait = slotE; break;
        }
        return portrait;
    }

    private PortraitComponent GetPortrait(SpeakerData speaker) {
        if (slotA.Speaker == speaker) return slotA;
        if (slotB.Speaker == speaker) return slotB;
        if (slotC.Speaker == speaker) return slotC;
        if (slotD.Speaker == speaker) return slotD;
        if (slotE.Speaker == speaker) return slotE;
        return null;
    }

    private List<PortraitComponent> GetPortraits() {
        return new List<PortraitComponent>() {
            slotA,
            slotB,
            slotC,
            slotD,
            slotE,
        };
    }
}
