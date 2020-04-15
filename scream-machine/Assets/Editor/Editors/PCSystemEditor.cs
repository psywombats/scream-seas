using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PCSystem))]
public class PCSystemEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        PCSystem pc = (PCSystem)target;
        if (GUILayout.Button("Hide")) {
            pc.Hide();
        }
        if (GUILayout.Button("Show")) {
            pc.Show();
        }
    }
}
