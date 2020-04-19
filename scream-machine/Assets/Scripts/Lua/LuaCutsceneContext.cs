using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;
using DG.Tweening;

public class LuaCutsceneContext : LuaContext {

    private static readonly string DefinesPath = "Lua/Defines/CutsceneDefines";

    public override IEnumerator RunRoutine(LuaScript script, bool canBlock) {
        if (canBlock && Global.Instance().Maps.Avatar != null) {
            Global.Instance().Maps.Avatar.PauseInput();
        }
        yield return base.RunRoutine(script, canBlock);
        if (MapOverlayUI.Instance().textbox.isDisplaying) {
            yield return MapOverlayUI.Instance().textbox.DisableRoutine();
        }
        if (canBlock && Global.Instance().Maps.Avatar != null) {
            Global.Instance().Maps.Avatar.UnpauseInput();
        }
    }

    public override void Initialize() {
        base.Initialize();
        LoadDefines(DefinesPath);
    }

    public override void RunRoutineFromLua(IEnumerator routine) {
        if (MapOverlayUI.Instance().textbox.isDisplaying) {
            // MapOverlayUI.Instance().textbox.MarkHiding();
            base.RunRoutineFromLua(CoUtils.RunSequence(new IEnumerator[] {
                MapOverlayUI.Instance().textbox.DisableRoutine(),
                routine,
            }));
        } else {
            base.RunRoutineFromLua(routine);
        }
    }

    public void RunTextboxRoutineFromLua(IEnumerator routine) {
        base.RunRoutineFromLua(routine);
    }

    protected void ResumeNextFrame() {
        Global.Instance().StartCoroutine(ResumeRoutine());
    }
    protected IEnumerator ResumeRoutine() {
        yield return null;
        ResumeAwaitedScript();
    }

    protected override void AssignGlobals() {
        base.AssignGlobals();
        lua.Globals["playBGM"] = (Action<DynValue>)PlayBGM;
        lua.Globals["playSound"] = (Action<DynValue>)PlaySound;
        lua.Globals["sceneSwitch"] = (Action<DynValue, DynValue>)SetSwitch;
        lua.Globals["face"] = (Action<DynValue, DynValue>)Face;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_walk"] = (Action<DynValue, DynValue, DynValue, DynValue>)Walk;
        lua.Globals["cs_diag"] = (Action<DynValue>)Diag;
        lua.Globals["cs_ladder"] = (Action<DynValue, DynValue>)Ladder;
        lua.Globals["cs_path"] = (Action<DynValue, DynValue, DynValue, DynValue>)Path;
        lua.Globals["cs_pathEvent"] = (Action<DynValue, DynValue, DynValue>)PathEvent;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        lua.Globals["cs_pc"] = (Action)StartPC;
        lua.Globals["cs_clientName"] = (Action<DynValue, DynValue>)SetClientName;
        lua.Globals["cs_message"] = (Action<DynValue, DynValue, DynValue>)Message;
        lua.Globals["cs_foreign"] = (Action<DynValue>)ForeignPhone;
        lua.Globals["cs_video"] = (Action)Video;
        lua.Globals["cs_flip"] = (Action)Flip;
        lua.Globals["cs_awaitBGMLoad"] = (Action)AwaitBGM;
        lua.Globals["cs_forceConversation"] = (Action<DynValue>)ForceConversation;

        lua.Globals["setNextScript"] = (Action<DynValue, DynValue, DynValue>)SetNextScript;
        lua.Globals["setNews"] = (Action<DynValue>)SetNews;
        lua.Globals["setSlideshow"] = (Action<DynValue>)SetSlideshow;
        lua.Globals["setSpeedMult"] = (Action<DynValue>)SetSpeedMult;
        lua.Globals["switchToSelectMode"] = (Action)SwitchToSelectMode;
        lua.Globals["messagePreview"] = (Action<DynValue, DynValue>)PreviewMessage;
        lua.Globals["panPhone"] = (Action)PanPhone;
        lua.Globals["playEnding"] = (Action)PlayEnding;
        lua.Globals["clearConversations"] = (Action)ClearConversations;
    }

    // === LUA CALLABLE ============================================================================

    private void PlayBGM(DynValue bgmKey) {
        Global.Instance().Audio.PlayBGM(bgmKey.String);
    }

    private void PlaySound(DynValue soundKey) {
        Global.Instance().Audio.PlaySFX(soundKey.String);
    }

    private void Teleport(DynValue mapName, DynValue x, DynValue y, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var loc = new Vector2Int((int)x.Number, (int)y.Number);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(Global.Instance().Maps.TeleportRoutine(mapName.String, loc, facing, raw));
    }

    private void TargetTeleport(DynValue mapName, DynValue targetEventName, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(Global.Instance().Maps.TeleportRoutine(mapName.String, targetEventName.String, facing, raw));
    }

    private void FadeOutBGM(DynValue seconds) {
        RunRoutineFromLua(Global.Instance().Audio.FadeOutRoutine((float)seconds.Number));
    }

    private void Speak(DynValue speaker, DynValue text) {
        var speakerString = speaker.IsNil() ? null : speaker.String;
        var textString = text.IsNil() ? null : text.String;
        if (speaker.String.Contains(":")) {
            textString = speakerString.Split(':')[1].Substring(2);
            speakerString = speakerString.Split(':')[0];
        }
        RunTextboxRoutineFromLua(MapOverlayUI.Instance().textbox.SpeakRoutine(speakerString, textString));
    }

    private void Walk(DynValue eventLua, DynValue steps, DynValue directionLua, DynValue waitLua) {
        if (eventLua.Type == DataType.String) {
            var @event = Global.Instance().Maps.ActiveMap.GetEventNamed(eventLua.String);
            if (@event == null) {
                Debug.LogError("Couldn't find event " + eventLua.String);
            } else {
                var routine = @event.StepMultiRoutine(OrthoDirExtensions.Parse(directionLua.String), (int)steps.Number);
                if (!waitLua.IsNil() && waitLua.Boolean) {
                    RunRoutineFromLua(routine);
                } else {
                    @event.StartCoroutine(routine);
                }
            }
        } else {
            var function = eventLua.Table.Get("walk");
            function.Function.Call(steps, directionLua, waitLua);
        }
    }

    private void Path(DynValue eventLua, DynValue targetArg1, DynValue targetArg2, DynValue targetArg3) {
        bool wait;
        Vector2Int target;
        if (targetArg1.Type == DataType.String) {
            var targetEvent = Global.Instance().Maps.ActiveMap.GetEventNamed(targetArg1.String);
            if (targetEvent == null) {
                Debug.LogError("Couldn't find event " + targetArg1.String);
                return;
            }
            target = targetEvent.Position;
            wait = targetArg2.Boolean;
        } else {
            target = new Vector2Int((int)targetArg1.Number, (int)targetArg2.Number);
            wait = targetArg3.Boolean;
        }

        if (eventLua.Type == DataType.String) {
            var @event = Global.Instance().Maps.ActiveMap.GetEventNamed(eventLua.String);
            if (@event == null) {
                Debug.LogError("Couldn't find event " + eventLua.String);
                return;
            }

            var routine = @event.PathToRoutine(target);
            if (wait) {
                RunRoutineFromLua(routine);
            } else {
                @event.StartCoroutine(routine);
                ResumeNextFrame();
            }
        } else {
            var function = eventLua.Table.Get("path");
            function.Function.Call(eventLua, targetArg1, targetArg2, targetArg3);
        }
    }

    private void PathEvent(DynValue moverLua, DynValue targetLua, DynValue waitLua) {
        var map = Global.Instance().Maps.ActiveMap;
        var mover = map.GetEventNamed(moverLua.String);
        var target = map.GetEventNamed(targetLua.String);
        var routine = mover.PathToRoutine(target.Position, ignoreEvents:true);
        if (waitLua.IsNil() || waitLua.Boolean) {
            RunRoutineFromLua(routine);
        } else {
            mover.StartCoroutine(routine);
        }
    }

    private void Face(DynValue eventName, DynValue dir) {
        var @event = Global.Instance().Maps.ActiveMap.GetEventNamed(eventName.String);
        if (@event == null) {
            Debug.LogError("Couldn't find event " + eventName.String);
        } else {
            @event.GetComponent<CharaEvent>().Facing = OrthoDirExtensions.Parse(dir.String);
        }
    }

    private FadeData lastFade;
    private void Fade(DynValue type) {
        var typeString = type.String;
        FadeData fade;
        bool invert = false;
        if (typeString == "normal") {
            fade = lastFade;
            invert = true;
        } else {
            fade = IndexDatabase.Instance().Fades.GetData(typeString);
        }
        lastFade = fade;
        var globals = Global.Instance();
        RunRoutineFromLua(globals.Maps.Camera.GetComponent<FadeComponent>().FadeRoutine(fade, invert));
    }

    private void Dummy(DynValue othername) {

    }

    private void SetClientName(DynValue nameLua, DynValue auto) {
        Global.Instance().Messenger.ActiveConvo.Client.displayName = nameLua.String;
        Global.Instance().Messenger.UpdateFromMessenger();
        if (auto.IsNil()) {
            RunRoutineFromLua(Global.Instance().Input.ConfirmRoutine());
        }
    }

    private void SetNextScript(DynValue scriptNameLua, DynValue prereadLua, DynValue delayLua) {
        Global.Instance().Messenger.SetNextScript(
            scriptNameLua.String,
            prereadLua.IsNil() ? false : prereadLua.Boolean,
            delayLua.IsNil() ? 0.0f : (float) delayLua.Number
            );
    }

    private void Message(DynValue senderLua, DynValue textLua, DynValue autotypeLua) {
        var phone = MapOverlayUI.Instance().phoneSystem.IsFlipped ? MapOverlayUI.Instance().bigPhone : MapOverlayUI.Instance().foreignPhone;
        if (autotypeLua.IsNil()) {
            RunRoutineFromLua(phone.PlayMessageRoutine(senderLua.String, textLua.String));
        } else {
            var type = autotypeLua.String;
            var delay = 4.4f;
            if (type == "short") delay = 2.8f;
            if (type == "long") delay = 6.5f;
            if (type == "micro") delay = 0.8f;
            RunRoutineFromLua(phone.PlayMessageRoutine(senderLua.String, textLua.String, delay));
        }
    }

    private void ForeignPhone(DynValue keyLua) {
        RunRoutineFromLua(Global.Instance().MessengerManager.ForeignPhoneRoutine(keyLua.String));
    }

    private void Video() {
        RunRoutineFromLua(MapOverlayUI.Instance().bigPhone.VideoRoutine());
    }

    private void Flip() {
        if (MapOverlayUI.Instance().phoneSystem.IsFlipped) {
            RunRoutineFromLua(MapOverlayUI.Instance().phoneSystem.FlipRoutine());
        } else {
            RunRoutineFromLua(AvatarEvent.PhoneRoutine());
        }
    }

    private void StartPC() {
        RunRoutineFromLua(CoUtils.TaskAsRoutine(MapOverlayUI.Instance().pcSystem.DoMenuAsync()));
    }

    private void SetNews(DynValue keyLua) {
        MapOverlayUI.Instance().pcSystem.SetNewsModel(IndexDatabase.Instance().PCNews.GetData(keyLua.String));
    }

    private void SetSlideshow(DynValue keyLua) {
        MapOverlayUI.Instance().pcSystem.SetSlideshowModel(IndexDatabase.Instance().PCSlideshows.GetData(keyLua.String));
    }

    private void Diag(DynValue dirLua) {
        OrthoDir dir = OrthoDirExtensions.Parse(dirLua.String);
        RunRoutineFromLua(Global.Instance().Maps.Avatar.Event.DiagRoutine(dir));
    }

    private void Ladder(DynValue countLua, DynValue dirLua) {
        OrthoDir dir = OrthoDirExtensions.Parse(dirLua.String);
        RunRoutineFromLua(Global.Instance().Maps.Avatar.Event.LadderRoutine((int) countLua.Number, dir));
    }

    private void AwaitBGM() {
        RunRoutineFromLua(Global.Instance().Audio.AwaitBGMLoad());
    }

    private void SetSpeedMult(DynValue multLua) {
        Global.Instance().Maps.Avatar.Event.SpeedMult = (float) multLua.Number;
    }

    private void SwitchToSelectMode() {
        MapOverlayUI.Instance().bigPhone.SwitchToSelectMode();
    }

    private void ForceConversation(DynValue clientLua) {
        var convo = Global.Instance().Messenger.GetConversation(clientLua.String);
        RunRoutineFromLua(CoUtils.TaskAsRoutine(MapOverlayUI.Instance().bigPhone.ShowConversation(convo)));
    }

    private void PreviewMessage(DynValue clientLua, DynValue textLua) {
        var client = Global.Instance().Messenger.GetClient(clientLua.String);
        var convo = Global.Instance().Messenger.GetConversation(client);
        convo.ForcePreview(textLua.String);
    }

    private void PanPhone() {
        MapOverlayUI.Instance().bigPhone.gameObject.transform.DOLocalMove(new Vector3(-410, 0), 1.0f).Play();
    }

    private void PlayEnding() {
        UnityEngine.Object.FindObjectOfType<EndingView>().PlayEnding();
    }

    private void ClearConversations() {
        Global.Instance().Messenger.Clear();
    }
}
