using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LuaCutsceneContext))]
public class LuaContextEditor : Editor {

    private string customLua;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        LuaContext context = (LuaContext)target;
        
        if (Application.IsPlaying(context)) {
            GUILayout.Space(12);

            if (!context.IsRunningScript()) {
                EditorGUILayout.LabelField("Lua debug prompt!");
            } else {
                EditorGUILayout.LabelField("Running...");
                EditorGUI.BeginDisabledGroup(true);
            }
            
            customLua = EditorGUILayout.TextArea(customLua, new GUILayoutOption[] { GUILayout.Height(120) });
            GUILayout.Space(12);

            if (!context.IsRunningScript()) {
                if (GUILayout.Button("Run")) {
                    LuaScript script = new LuaScript(context, customLua);
                    context.StartCoroutine(script.RunRoutine());
                }
            } else {
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Force terminate")) {
                    context.ForceTerminate();
                }
            }
            GUILayout.Space(6);
        }
    }

    public override bool RequiresConstantRepaint() {
        LuaContext context = (LuaContext)target;
        return base.RequiresConstantRepaint() || context.IsRunningScript();
    }
}
