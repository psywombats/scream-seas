using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Coroutine = MoonSharp.Interpreter.Coroutine;
using System;
using System.Collections.Generic;

/// <summary>
///  A wrapper around Script that represents an environment where a script can execute.
/// </summary>
public class LuaContext {
    
    private const string DefinesPath = "Lua/Defines/GlobalDefines";
    private const string ScenesPath = "Lua/Scenes";

    private static string defines;

    private Script _lua;
    public Script lua {
        get {
            if (_lua == null) {
                _lua = new Script();
            }
            return _lua;
        }
    }

    private Stack<LuaScript> activeScripts = new Stack<LuaScript>();
    private bool forceKilled;

    public virtual void Initialize() {
        LoadDefines(DefinesPath);
        AssignGlobals();
    }

    // make sure the luaobject has been registered via [MoonSharpUserData]
    public void SetGlobal(string key, object luaObject) {
        lua.Globals[key] = luaObject;
    }

    public bool IsRunning() {
        return activeScripts.Count > 0;
    }

    public DynValue CreateObject() {
        return lua.DoString("return {}");
    }

    public DynValue Marshal(object toMarshal) {
        // touch globals to make sure assembly registered
        Global.Instance();

        return DynValue.FromObject(lua, toMarshal);
    }

    public Coroutine CreateScript(string fullScript) {
        try {
            DynValue scriptFunction = lua.DoString(fullScript);
            return lua.CreateCoroutine(scriptFunction).Coroutine;
        } catch (SyntaxErrorException e) {
            Debug.LogError("bad script: " + fullScript + "\n\nerror:\n" + e.DecoratedMessage);
            throw e;
        }
    }

    // all coroutines that are meant to block execution of the script should go through here
    public virtual void RunRoutineFromLua(IEnumerator routine) {
        if (forceKilled) {
            // leave the old instance infinitely suspended
            return;
        }
        Global.Instance().StartCoroutine(CoUtils.RunWithCallback(routine, () => {
            if (activeScripts.Count > 0 && !forceKilled) {
                ResumeAwaitedScript();
            }
        }));
    }

    // meant to be evaluated synchronously
    public LuaCondition CreateCondition(string luaScript) {
        return new LuaCondition(this, lua.LoadString(luaScript));
    }

    // evaluates a lua function in the global context
    public DynValue Evaluate(DynValue function) {
        return lua.Call(function);
    }

    // hang on to a chunk of lua to run later
    public DynValue Load(string luaChunk) {
        return lua.LoadString(luaChunk);
    }

    // kills the current script, useful for debug only
    public void ForceTerminate() {
        forceKilled = true;
    }

    public IEnumerator RunRoutine(string luaString, bool canBlock) {
        LuaScript script = new LuaScript(this, luaString);
        yield return RunRoutine(script, canBlock);
    }

    public virtual IEnumerator RunRoutine(LuaScript script, bool canBlock) {
        activeScripts.Push(script);
        forceKilled = false;
        try {
            script.scriptRoutine.Resume();
        } catch (Exception) {
            Debug.Log("Exception during script: " + script + "\n context: " + this);
            throw;
        }
        while (script.scriptRoutine.State != CoroutineState.Dead && !forceKilled) {
            yield return null;
        }
        activeScripts.Pop();
    }

    public IEnumerator RunRoutineFromFile(string filename, bool canBlock = true) {
        if (filename.Contains(".")) {
            filename = filename.Substring(0, filename.IndexOf('.'));
        }
        var asset = Resources.Load<LuaSerializedScript>("Lua/" + filename);
        yield return RunRoutine(asset.luaString, canBlock);
    }

    protected void ResumeAwaitedScript() {
        activeScripts.Peek().scriptRoutine.Resume();
    }

    protected virtual void AssignGlobals() {
        lua.Globals["debugLog"] = (Action<DynValue>)DebugLog;
        lua.Globals["playSFX"] = (Action<DynValue>)PlaySFX;
        lua.Globals["cs_wait"] = (Action<DynValue>)Wait;
        lua.Globals["cs_play"] = (Action<DynValue, DynValue>)Play;
        lua.Globals["playSceneParallel"] = (Action<DynValue>)PlaySceneParallel;
        lua.Globals["getSwitch"] = (Func<DynValue, DynValue>)GetSwitch;
        lua.Globals["setSwitch"] = (Action<DynValue, DynValue>)SetSwitch;
        lua.Globals["eventNamed"] = (Func<DynValue, LuaMapEvent>)EventNamed;
        lua.Globals["getAvatar"] = (Func<DynValue>)GetAvatar;
        lua.Globals["rand"] = (Func<DynValue, DynValue>)Rand;
        lua.Globals["isBigRoom"] = (Func<DynValue>)IsBigMap;
    }

    protected void LoadDefines(string path) {
        LuaSerializedScript script = Resources.Load<LuaSerializedScript>(path);
        lua.DoString(script.luaString);
    }

    // === LUA CALLABLE ============================================================================

    protected DynValue IsBigMap() {
        var map = Global.Instance().Maps.ActiveMap;
        return Marshal(map.size.x >= 10 && map.size.x <= 20);
    }

    protected LuaMapEvent EventNamed(DynValue eventName) {
        MapEvent mapEvent = Global.Instance().Maps.ActiveMap.GetEventNamed(eventName.String);
        if (mapEvent == null) {
            return null;
        } else {
            return mapEvent.LuaObject;
        }
    }

    protected DynValue GetSwitch(DynValue switchName) {
        bool value = Global.Instance().Data.GetSwitch(switchName.String);
        return Marshal(value);
    }

    protected void SetSwitch(DynValue switchName, DynValue value) {
        Global.Instance().Data.SetSwitch(switchName.String, value.Boolean);
    }

    protected void DebugLog(DynValue message) {
        Debug.Log(message.CastToString());
    }

    protected void Wait(DynValue seconds) {
        RunRoutineFromLua(CoUtils.Wait((float)seconds.Number));
    }

    protected void PlaySFX(DynValue sfxKey) {
        Global.Instance().Audio.PlaySFX(sfxKey.String);
    }

    protected DynValue Rand(DynValue max) {
        return Marshal(UnityEngine.Random.Range(0, (int)max.Number));
    }
    protected void Play(DynValue filename, DynValue delay) => Play(filename, delay, false);
    protected void Play(DynValue filename, DynValue delay, bool blocks = true) {
        if (delay.IsNil()) {
            RunRoutineFromLua(RunRoutineFromFile(filename.String, blocks));
        } else {
            RunRoutineFromLua(CoUtils.Wait(0.01f));
            activeScripts.Peek().scriptRoutine.Resume();
            Global.Instance().StartCoroutine(CoUtils.RunAfterDelay((float)delay.Number, () => {
                Global.Instance().StartCoroutine(RunRoutineFromFile(filename.String));
            }));
        }
    }

    protected void PlaySceneParallel(DynValue filename) {
        Global.Instance().StartCoroutine(UglyRoutine());
    }

    protected DynValue GetAvatar() {
        var obj = Global.Instance().Maps.Avatar.Event.LuaObject;
        return UserData.Create(obj);
    }

    private IEnumerator UglyRoutine() {
        var commands = new List<Action>() {
            () => PreviewMessage("SKETCH", "Run. Get out. Run. Get out."),
            () => PreviewMessage("CONTROL", "Ahahah! I get it, I understand all of it, written here!"),
            () => PreviewMessage("SIS", "***There is no need to be worried or afraid"),
            () => PreviewMessage("SKETCH", "Leave GoodWin. He is no longer human. Run. Run Run."),
            () => PreviewMessage("CONTROL", "We were acting to the Plan all along!"),
            () => PreviewMessage("SIS", "***Do you understand the Plan now?"),
            () => PreviewMessage("SKETCH", "Run. Now. Do not listen to anyone but me."),
            () => PreviewMessage("CONTROL", "Every move I made, He was one step ahead. Brilliant!"),
            () => PreviewMessage("SIS", "***You see it now, don't you, that I am the Vertigo Temple?"),
            () => PreviewMessage("SKETCH", "I may be too late. Are you still in the physical world?"),
            () => PreviewMessage("CONTROL", "Toooo good!!! It fits! The message! Signs! All of it!"),
            () => PreviewMessage("SIS", "***You can discard your shell now - come join us"),
            () => PreviewMessage("SKETCH", "Elle left us long ago. She exists only within VT's simulation"),
            () => PreviewMessage("CONTROL", "The mystery at last!!"),
            () => PreviewMessage("SIS", "***If you understand the Plan, you have ascended"),
            () => PreviewMessage("SKETCH", "Run. Run run run run."),
            () => PreviewMessage("CONTROL", "Sooo you've all betrayed me! Hahahahaha! Well done!"),
            () => PreviewMessage("SIS", "***Remember, I love you always"),
            () => PreviewMessage("SKETCH", "If you wish to remain in this world, you must escape."),
            () => PreviewMessage("CONTROL", "The Truth, the Split Second Instant Of Truth!"),
            () => PreviewMessage("SIS", "***Soon we will meet again"),
        };
        var offset = 0;
        while (!Global.Instance().Data.GetSwitch("stop_spam")) {
            if (offset >= commands.Count) {
                offset = 0;
            }
            commands[offset]();
            yield return CoUtils.Wait(1.166f * 3);
            offset += 1;
        }
    }
    private void PreviewMessage(string clientString, string text) {
        var client = Global.Instance().Messenger.GetClient(clientString);
        var convo = Global.Instance().Messenger.GetConversation(client);
        convo.ForcePreview(text);
    }
}
