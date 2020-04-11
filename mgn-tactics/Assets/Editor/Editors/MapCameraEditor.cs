using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapCamera2D))]
public class MapCameraEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        MapCamera2D camera = (MapCamera2D)target;
        if (GUILayout.Button("Attach and Center")) {
            AvatarEvent avatar = FindObjectOfType<AvatarEvent>();
            if (avatar != null) {
                camera.target = avatar.GetComponent<MapEvent>();
                camera.ManualUpdate();
                EditorUtility.SetDirty(camera);
            } else {
                Debug.LogError("No avatar could be found in the scene");
            }
        }
    }
}
