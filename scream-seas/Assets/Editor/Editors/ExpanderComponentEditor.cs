using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ExpanderComponent), editorForChildClasses: true)]
public class ExpanderComponentEditor : Editor {

    private bool useCoroutines;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var component = (ExpanderComponent)target;

        if (!Application.isPlaying) {
            if (GUILayout.Button("Memorize Min Size")) {
                component.minSize = component.RectTransform.sizeDelta;
            }

            if (GUILayout.Button("Memorize Max Size")) {
                component.maxSize = component.RectTransform.sizeDelta;
            }
        } else {
            useCoroutines = GUILayout.Toggle(useCoroutines, "Use coroutines");
        }

        if (GUILayout.Button("Hide")) {
            if (useCoroutines) {
                component.StartCoroutine(component.HideRoutine());
            } else {
                component.Hide();
            }
        }

        if (GUILayout.Button("Show")) {
            if (useCoroutines) {
                component.gameObject.SetActive(true);
                component.StartCoroutine(component.ShowRoutine());
            } else {
                component.Show();
            }
        }
    }
}
