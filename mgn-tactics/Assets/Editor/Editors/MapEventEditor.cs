using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEvent), true)]
public class MapEventEditor : Editor {

    private static readonly string DollPath = "Assets/Resources/Prefabs/Doll.prefab";

    public override void OnInspectorGUI() {
        MapEvent mapEvent = (MapEvent)target;

        if (GUI.changed) {
            mapEvent.SetScreenPositionToMatchTilePosition();
            mapEvent.SetDepth();
        }

        if (!mapEvent.GetComponent<CharaEvent>()) {
            if (GUILayout.Button("Add Chara Event")) {
                GameObject doll = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(DollPath));
                doll.name = mapEvent.name + " (doll)";
                GameObjectUtility.SetParentAndAlign(doll, mapEvent.gameObject);
                CharaEvent chara = mapEvent.gameObject.AddComponent<CharaEvent>();
                chara.doll = doll;
                mapEvent.passable = false;
                Undo.RegisterCreatedObjectUndo(mapEvent, "Create " + doll.name);
                Selection.activeObject = doll;

                // hardcode weirdness
                doll.transform.localPosition = new Vector3(Map.TileSizePx / 2, -Map.TileSizePx, 0.0f);
            }
            GUILayout.Space(25.0f);
        }

        if (Application.IsPlaying(target) && GUILayout.Button("Regenerate Lua")) {
            mapEvent.GenerateLua();
        }

        Vector2Int originalPosition = mapEvent.position;

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.position);
        if (newPosition != mapEvent.position) {
            Undo.RecordObject(mapEvent, "Reposition event");
            mapEvent.SetLocation(newPosition);
        }

        Vector2Int newSize = EditorGUILayout.Vector2IntField("Size", mapEvent.size);
        if (newSize != mapEvent.size) {
            Undo.RecordObject(mapEvent, "Resize event");
            mapEvent.SetSize(newSize);
        }

        if (!Application.IsPlaying(mapEvent)) {
            mapEvent.SetTilePositionToMatchScreenPosition();
        }

        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        MapEvent mapEvent = (MapEvent)target;
        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(mapEvent.GetHandlePosition(), Quaternion.identity);
        newPosition += (mapEvent.transform.position - mapEvent.GetHandlePosition());
        if (EditorGUI.EndChangeCheck()) {
            Undo.RegisterCompleteObjectUndo(mapEvent.transform, "Drag " + mapEvent);
            mapEvent.transform.localPosition = newPosition;
            mapEvent.SetTilePositionToMatchScreenPosition();
            serializedObject.FindProperty("position").vector2IntValue = mapEvent.position;
            serializedObject.ApplyModifiedProperties();
        }
        
    }

    public void OnEnable() {
        Tools.hidden = true;
    }

    public void OnDisable() {
        Tools.hidden = false;
    }
}
