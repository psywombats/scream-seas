using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SmsScript))]
public class SmsScriptEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SmsScript script = (SmsScript)target;

        if (!Application.isPlaying && GUILayout.Button("Infer preview")) {
            script.InferPreview();
        }
    }
}
