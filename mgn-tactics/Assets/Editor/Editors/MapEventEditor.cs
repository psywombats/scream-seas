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
                /*
                GameObject doll = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(DollPath));
                doll.name = mapEvent.name + " (doll)";
                GameObjectUtility.SetParentAndAlign(doll, mapEvent.gameObject);
                CharaEvent chara = mapEvent.gameObject.AddComponent<CharaEvent>();
                chara.Doll = doll;
                mapEvent.Passable = false;
                Undo.RegisterCreatedObjectUndo(mapEvent, "Create " + doll.name);
                Selection.activeObject = doll;

                // hardcode weirdness
                doll.transform.localPosition = new Vector3(Map.TileSizePx / 2, -Map.TileSizePx, 0.0f);
                */
            }
            GUILayout.Space(25.0f);
        }

        if (Application.IsPlaying(target) && GUILayout.Button("Regenerate Lua")) {
            mapEvent.GenerateLua();
        }

        Vector2Int originalPosition = mapEvent.Position;

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.Position);
        if (newPosition != mapEvent.Position) {
            Undo.RecordObject(mapEvent, "Reposition event");
            mapEvent.SetPosition(newPosition);
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
            serializedObject.FindProperty("Position").vector2IntValue = mapEvent.Position;
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
