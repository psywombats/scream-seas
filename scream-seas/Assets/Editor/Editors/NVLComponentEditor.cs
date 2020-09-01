using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NVLComponent), editorForChildClasses: true)]
public class NVLComponentEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var component = (NVLComponent)target;

        if (Application.isPlaying) {
            if (GUILayout.Button("Debug")) {
                Global.Instance().StartCoroutine(DebugRoutine(component));
            }
        }
    }

    private IEnumerator DebugRoutine(NVLComponent nvl) {
        yield return nvl.ShowRoutine();
    }
}
