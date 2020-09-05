using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All MapEvents own a lua equivalent that has functions stored on it
/// </summary>
[MoonSharpUserData]
public class LuaMapEvent {

    [MoonSharpHidden] private MapEvent mapEvent;
    [MoonSharpHidden] private Dictionary<string, DynValue> values;

    [MoonSharpHidden]
    public LuaMapEvent(MapEvent mapEvent) {
        this.mapEvent = mapEvent;
        values = new Dictionary<string, DynValue>();
    }

    // meant to be called with the key/value of a lualike property on a Tiled object
    // accepts nil and zero-length as no-ops
    [MoonSharpHidden]
    public void Set(string name, string luaChunk) {
        if (luaChunk != null && luaChunk.Length > 0) {
            var context = Global.Instance().Maps.Lua;
            values[name] = context.Load(luaChunk);
        }
    }

    [MoonSharpHidden]
    public void Run(string eventName, bool canBlock = true) {
        if (values.ContainsKey(eventName) && values[eventName] != null) {
            var context = Global.Instance().Maps.Lua;
            var luaValue = context.Marshal(this);
            context.lua.Globals["this"] = luaValue;
            LuaScript script = new LuaScript(context, values[eventName]);
            Global.Instance().StartCoroutine(script.RunRoutine(canBlock));
        }
    }

    [MoonSharpHidden]
    public DynValue Evaluate(string propertyName) {
        if (!values.ContainsKey(propertyName)) {
            return DynValue.Nil;
        } else {
            var context = Global.Instance().Maps.Lua;
            return context.Evaluate(values[propertyName]);
        }
    }

    [MoonSharpHidden]
    public bool EvaluateBool(string propertyName, bool defaultValue = false) {
        if (!values.ContainsKey(propertyName)) {
            return defaultValue;
        } else {
            DynValue result = Evaluate(propertyName);
            return result.Boolean;
        }
    }

    // === CALLED BY LUA === 

    public void face(string directionName) {
        mapEvent.GetComponent<CharaEvent>().Facing = OrthoDirExtensions.Parse(directionName);
    }

    public void faceToward(LuaMapEvent other) {
        mapEvent.GetComponent<CharaEvent>().Facing = mapEvent.DirectionTo(other.mapEvent);
    }

    public int x() {
        return mapEvent.Position.x;
    }

    public int y() {
        return mapEvent.Position.y;
    }

    public string facing() {
        return mapEvent.GetComponent<CharaEvent>().Facing.DirectionName().ToUpper();
    }

    public void debuglog() {
        Debug.Log("Debug: " + mapEvent.name);
    }

    public void path(int x, int y, bool wait = false) {
        var routine = mapEvent.PathToRoutine(new Vector2Int(x, y));
        if (wait) {
            var context = Global.Instance().Maps.Lua;
            context.RunRoutineFromLua(routine);
        } else {
            mapEvent.StartCoroutine(routine);
        }
    }

    public void walk(string directionName, int count) {
        var context = Global.Instance().Maps.Lua;
        context.RunRoutineFromLua(mapEvent.StepMultiRoutine(OrthoDirExtensions.Parse(directionName), count));
    }

    public void wander() {
        var offset = UnityEngine.Random.Range(0, 4);
        for (var @base = 0; @base < 4; @base += 1) {
            var ordinal = (offset + @base) % 4;
            var dir = (OrthoDir)ordinal;
            if (dir.Px2DX() == 0) continue;
            var newPos = mapEvent.Position + dir.XY2D();
            if (mapEvent.Map.IsChipPassableAt(newPos)) {
                if (Global.Instance().Maps.Avatar.Event.Position == newPos) {
                    mapEvent.GetComponent<CharaEvent>().Facing = dir;
                    var context = Global.Instance().Maps.Lua;
                    Run(MapEvent.PropertyLuaCollide);
                    break;
                } else if (mapEvent.CanPassAt(newPos) && mapEvent.Map.GetEventAt<MapEvent>(newPos) == null) {
                    var context = Global.Instance().Maps.Lua;
                    mapEvent.StartCoroutine(mapEvent.StepRoutine(dir));
                    break;
                }
            }
        }
    }

    public void stalk() {
        if (CharaEvent.disableStalk) return;
        OrthoDir dir;
        var dist = (mapEvent.Position - Global.Instance().Maps.Avatar.Event.Position).magnitude;
        if (dist > 12) return;
        var tooClose = false;
        if (mapEvent.Position.x > Global.Instance().Maps.Avatar.Event.Position.x ^ tooClose) {
            dir = OrthoDir.West;
        } else {
            dir = OrthoDir.East;
        }
        var newPos = mapEvent.Position + dir.XY2D();
        if (mapEvent.CanPassAt(newPos) && mapEvent.Map.GetEventAt<MapEvent>(newPos) == null) {
            var context = Global.Instance().Maps.Lua;
            mapEvent.StartCoroutine(mapEvent.StepRoutine(dir));
        }
    }

    public void stalk2() {
        OrthoDir dir;
        var dist = (mapEvent.Position - Global.Instance().Maps.Avatar.Event.Position).magnitude;
        if (dist > 5) return;
        var tooClose = dist < 2;
        if (mapEvent.Position.x > Global.Instance().Maps.Avatar.Event.Position.x ^ tooClose) {
            dir = OrthoDir.West;
        } else {
            dir = OrthoDir.East;
        }
        var newPos = mapEvent.Position + dir.XY2D();
        if (mapEvent.CanPassAt(newPos) && mapEvent.Map.GetEventAt<MapEvent>(newPos) == null) {
            var context = Global.Instance().Maps.Lua;
            mapEvent.StartCoroutine(mapEvent.StepRoutine(dir));
        }
    }

    public void cs_step(string directionName) {
        var context = Global.Instance().Maps.Lua;
        context.RunRoutineFromLua(mapEvent.StepRoutine(OrthoDirExtensions.Parse(directionName)));
    }
}
