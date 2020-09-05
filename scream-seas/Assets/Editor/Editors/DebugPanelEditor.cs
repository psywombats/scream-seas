using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugPanel))]
public class DebugPanelEditor : Editor {

    private string customLua;

    private LuaContext Lua => Global.Instance().Maps.Lua;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (Application.IsPlaying(Global.Instance())) {
            GUILayout.Space(12);

            GUILayout.Label("Current location: " + Global.Instance().Maps.ActiveMap.InternalName);

            GUILayout.Space(36);

            var panel = (DebugPanel)target;

            if (!Lua.IsRunning()) {
                EditorGUILayout.LabelField("Lua debug prompt!");
            } else {
                EditorGUILayout.LabelField("Running...");
                EditorGUI.BeginDisabledGroup(true);
            }

            customLua = EditorGUILayout.TextArea(customLua, new GUILayoutOption[] { GUILayout.Height(120) });
            GUILayout.Space(12);

            if (Lua.IsRunning()) {
                EditorGUILayout.LabelField("Running...");
                EditorGUI.EndDisabledGroup();
            }

            if (!Lua.IsRunning()) {
                if (GUILayout.Button("Run")) {
                    LuaScript script = new LuaScript(Lua, customLua);
                    Global.Instance().StartCoroutine(script.RunRoutine(true));
                }
            } else {
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Force terminate")) {
                    Lua.ForceTerminate();
                }
            }
            GUILayout.Space(32);

            EditorGUILayout.LabelField("Walk speed mult");
            panel.WalkSpeedMult = GUILayout.HorizontalSlider(panel.WalkSpeedMult, 0.0f, 10.0f);

            GUILayout.Space(8);
            
            GUILayout.Space(8);
        }
    }

    public override bool RequiresConstantRepaint() {
        return base.RequiresConstantRepaint() || Lua.IsRunning();
    }
}
