using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;

public class LuaCutsceneContext : LuaContext {

    private static readonly string DefinesPath = "Lua/Defines/CutsceneDefines";

    public override IEnumerator RunRoutine(LuaScript script, bool canBlock) {
        if (canBlock && Global.Instance().Maps.Avatar != null) {
            Global.Instance().Maps.Avatar.PauseInput();
        }
        yield return base.RunRoutine(script, canBlock);
        if (canBlock && Global.Instance().Maps.Avatar != null) {
            Global.Instance().Maps.Avatar.UnpauseInput();
        }
    }

    public override void Initialize() {
        base.Initialize();
        LoadDefines(DefinesPath);
    }

    public override void RunRoutineFromLua(IEnumerator routine) {
            base.RunRoutineFromLua(routine);
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
        lua.Globals["spawnChaser"] = (Action<DynValue, DynValue>)SpawnChaser;
        lua.Globals["wipe"] = (Action)Wipe;
        lua.Globals["clear"] = (Action)Wipe;
        lua.Globals["chaserGameOver"] = (Action)ChaserGameOver;
        lua.Globals["slowtype"] = (Action<DynValue>)Slowtype;
        lua.Globals["cyPan"] = (Action)CyPan;
        lua.Globals["cyEat"] = (Action)CyEat;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_walk"] = (Action<DynValue, DynValue, DynValue, DynValue>)Walk;
        lua.Globals["cs_diag"] = (Action<DynValue>)Diag;
        lua.Globals["cs_ladder"] = (Action<DynValue, DynValue>)Ladder;
        lua.Globals["cs_path"] = (Action<DynValue, DynValue, DynValue, DynValue>)Path;
        lua.Globals["cs_pathEvent"] = (Action<DynValue, DynValue, DynValue>)PathEvent;
        //lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        lua.Globals["cs_awaitBGMLoad"] = (Action)AwaitBGM;
        lua.Globals["cs_enterNVL"] = (Action)EnterNVL;
        lua.Globals["cs_exitNVL"] = (Action)ExitNVL;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        lua.Globals["cs_exit"] = (Action<DynValue>)Exit;
        lua.Globals["cs_enter"] = (Action<DynValue, DynValue>)Enter;
        lua.Globals["cs_setBG"] = (Action<DynValue>)SetBG;
        lua.Globals["cs_shanty"] = (Action)Shanty;
        lua.Globals["cs_beginMessage"] = (Action)BeginMessage;
        lua.Globals["cs_message"] = (Action<DynValue>)Message;
        lua.Globals["cs_endMessage"] = (Action)EndMessage;
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

    //private void Speak(DynValue speaker, DynValue text) {
    //    var speakerString = speaker.IsNil() ? null : speaker.String;
    //    var textString = text.IsNil() ? null : text.String;
    //    if (speaker.String.Contains(":")) {
    //        textString = speakerString.Split(':')[1].Substring(2);
    //        speakerString = speakerString.Split(':')[0];
    //    }
    //    RunTextboxRoutineFromLua(MapOverlayUI.Instance().textbox.SpeakRoutine(speakerString, textString));
    //}

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
        RunRoutineFromLua(globals.Maps.Camera.fade.FadeRoutine(fade, invert));
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

    private void SpawnChaser(DynValue targetLua, DynValue delayLua) {
        Global.Instance().StartCoroutine(SpawnChaserRoutine(targetLua.String, delayLua.Number));
    }
    private IEnumerator SpawnChaserRoutine(string targetName, double delay) {
        var map = Global.Instance().Maps.ActiveMap;
        var target = map.GetEventNamed(targetName);
        yield return Global.Instance().Maps.SpawnChaserRoutine(map, target.Position.x, target.Position.y, (float)delay);
    }

    public void EnterNVL() {
        RunRoutineFromLua(EnterNVLRoutine());
    }
    private IEnumerator EnterNVLRoutine() {
        yield return MapOverlayUI.Instance().nvl.ShowRoutine();
    }

    public void ExitNVL() {
        RunRoutineFromLua(ExitNVLRoutine());
    }
    private IEnumerator ExitNVLRoutine() {
        yield return MapOverlayUI.Instance().nvl.HideRoutine();
    }

    public void Speak(DynValue speakerNameLua, DynValue messageLua) {
        var speaker = IndexDatabase.Instance().Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(SpeakRoutine(speaker, messageLua.String));
    }
    private IEnumerator SpeakRoutine(SpeakerData speaker, string message) {
        yield return MapOverlayUI.Instance().nvl.SpeakRoutine(speaker, message);
    }

    public void Enter(DynValue speakerNameLua, DynValue slotLua) {
        var speaker = IndexDatabase.Instance().Speakers.GetData(speakerNameLua.String);
        var slot = slotLua.String;
        RunRoutineFromLua(EnterRoutine(speaker, slot));
    }
    private IEnumerator EnterRoutine(SpeakerData speaker, string slot) {
        yield return MapOverlayUI.Instance().nvl.EnterRoutine(speaker, slot);
    }

    public void Exit(DynValue speakerNameLua) {
        var speaker = IndexDatabase.Instance().Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(ExitRoutine(speaker));
    }
    private IEnumerator ExitRoutine(SpeakerData speaker) {
        yield return MapOverlayUI.Instance().nvl.ExitRoutine(speaker);
    }

    public void SetBG(DynValue bgLua) {
        var bg = IndexDatabase.Instance().Backgrounds.GetData(bgLua.String);
        RunRoutineFromLua(SetBGRoutine(bg));

    }
    private IEnumerator SetBGRoutine(BackgroundData background) {
        yield return MapOverlayUI.Instance().nvl.SetBGRoutine(background.bg);
    }

    public void Wipe() {
        MapOverlayUI.Instance().nvl.Wipe();
    }

    private void Shanty() {
        RunRoutineFromLua(ShantyRoutine());
    }
    private IEnumerator ShantyRoutine() {
        yield return MapOverlayUI.Instance().shanty.StartRoutine();
        yield return MapOverlayUI.Instance().shanty.PlayRoutine(new string[] {
            "O Sailors, keep your secrets",
            "Hid inside your treasure chest",
            "O Sailors, keep your secrets",
            "Hid inside your treasure chest",
            "If the ocean ever finds the haul",
            "That's hidden in your chest",
            "She'll return it to the sea"
        });
        yield return MapOverlayUI.Instance().shanty.PlayRoutine(new string[] {
            "Well, I found a wayward soul",
            "As they was driftin' far away",
            "Well, I found a wayward soul",
            "As they was driftin' far away",
            "Lord, help me save their soul",
            "Afore they go too far astray",
            "And surrender to the sea"
        });
        yield return MapOverlayUI.Instance().shanty.PlayRoutine(new string[] {
            "O Confiteor Deo",
            "Let me tell thee what I know",
            "O Confiteor Deo",
            "Let me tell thee what I know",
            "If thou deign to save my soul",
            "Then I shall tell thee all I know",
            "Pray return it to the sea"
        });
        yield return MapOverlayUI.Instance().shanty.PlayRoutine(new string[] {
            "Well, I went and crossed me brother",
            "Though he trust me with his life",
            "Well, I went and crossed me brother",
            "Though he trust me with his life",
            "There'll be plenty time to think upon it",
            "In the afterlife,",
            "When I come home to the sea"
        });
        yield return MapOverlayUI.Instance().shanty.PlayRoutine(new string[] {
            "O Sailors, keep your secrets",
            "Hid inside your treasure chest",
            "O Sailors, keep your secrets",
            "Hid inside your treasure chest",
            "If the ocean ever finds the haul",
            "That's hidden in your chest",
            "She'll return it to the sea"
        });
        yield return MapOverlayUI.Instance().shanty.FinishRoutine();
    }

    private void ChaserGameOver() {
    }

    public void BeginMessage() {
        RunRoutineFromLua(MapOverlayUI.Instance().message.BeginRoutine());
    }

    public void EndMessage() {
        RunRoutineFromLua(MapOverlayUI.Instance().message.EndRoutine());
    }

    public void Message(DynValue messageLua) {
        RunRoutineFromLua(MapOverlayUI.Instance().message.TypeRoutine(messageLua.String));
    }

    public void Slowtype(DynValue charsPerSecondLua) {
        MapOverlayUI.Instance().nvl.text.charsPerSecond = (float) charsPerSecondLua.Number;
    }

    public void CyPan() {
        RunRoutineFromLua(UnityEngine.Object.FindObjectOfType<CyPanComponent>().PanRoutine());
    }

    public void CyEat() {
        RunRoutineFromLua(UnityEngine.Object.FindObjectOfType<CyPanComponent>().EatRoutine());
    }
}
