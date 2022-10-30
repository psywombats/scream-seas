using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All MapEvents own a lua equivalent that has functions stored on it
/// </summary>
public class LuaMapEvent {

    private MapEvent mapEvent;
    private Dictionary<string, DynValue> values;
    
    public LuaMapEvent(MapEvent mapEvent) {
        this.mapEvent = mapEvent;
        values = new Dictionary<string, DynValue>();
    }

    // meant to be called with the key/value of a lualike property on a Tiled object
    // accepts nil and zero-length as no-ops
    
    public void Set(string name, string luaChunk) {
        if (luaChunk != null && luaChunk.Length > 0) {
            var context = Global.Instance().Maps.Lua;
            values[name] = context.Load(luaChunk);
        }
    }
    
    public void Run(string eventName, bool canBlock = true) {
        if (values.ContainsKey(eventName) && values[eventName] != null) {
            var context = Global.Instance().Maps.Lua;
            LuaScript script = new LuaScript(context, values[eventName]);
            Global.Instance().StartCoroutine(script.RunRoutine(canBlock));
        }
    }
    
    public DynValue Evaluate(string propertyName) {
        if (!values.ContainsKey(propertyName)) {
            return DynValue.Nil;
        } else {
            var context = Global.Instance().Maps.Lua;
            return context.Evaluate(values[propertyName]);
        }
    }
    
    public bool EvaluateBool(string propertyName, bool defaultValue = false) {
        if (!values.ContainsKey(propertyName)) {
            return defaultValue;
        } else {
            DynValue result = Evaluate(propertyName);
            return result.Boolean;
        }
    }
}
