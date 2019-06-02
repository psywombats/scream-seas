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

        Vector2Int originalPosition = mapEvent.position;

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.position);
        if (newPosition != mapEvent.position) {
            mapEvent.SetLocation(newPosition);
        }

        Vector2Int newSize = EditorGUILayout.Vector2IntField("Size", mapEvent.size);
        if (newSize != mapEvent.size) {
            mapEvent.SetSize(newSize);
        }

        mapEvent.SetTilePositionToMatchScreenPosition();

        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        MapEvent mapEvent = (MapEvent)target;
        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(mapEvent.transform.position, Quaternion.identity);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RegisterCompleteObjectUndo(mapEvent.transform, "Drag " + mapEvent);
            Undo.RegisterCompleteObjectUndo(mapEvent, "Tile move " + mapEvent);
            mapEvent.transform.localPosition = newPosition;
        }
        
    }

    public void OnEnable() {
        Tools.hidden = true;
    }

    public void OnDisable() {
        Tools.hidden = false;
    }
}
