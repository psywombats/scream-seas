using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhoneSystem))]
public class PhoneSystemEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        PhoneSystem phone = (PhoneSystem)target;

        if (Application.isPlaying && GUILayout.Button("Flip!")) {
            phone.StartCoroutine(phone.FlipRoutine());
        }
        if (Application.isPlaying && GUILayout.Button("Reset")) {
            phone.Reset();
        }
    }
}
