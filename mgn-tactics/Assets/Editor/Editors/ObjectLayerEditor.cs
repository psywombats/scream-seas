using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectLayer))]
public class ObjectLayerEditor : Editor {

    private static readonly string GenericPrefabPath = "Assets/Resources/Prefabs/MapEvent2D.prefab";

    private Vector2Int lastMouseTiles;

    public void OnSceneGUI() {


        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        EventType typeForControl = Event.current.GetTypeForControl(controlId);
        switch (Event.current.button) {
            case 1:
                HandleRightclick(typeForControl, controlId);
                break;
        }
    }

    private void HandleRightclick(EventType typeForControl, int controlId) {
        ObjectLayer layer = (ObjectLayer)target;

        switch (typeForControl) {
            case EventType.MouseDown:
                lastMouseTiles = GetCurrentMouseTiles();
                break;
            case EventType.MouseUp:
                // would indicate a click, vs a drag
                if (lastMouseTiles == GetCurrentMouseTiles() && IsLegal(lastMouseTiles)) {
                    ShowContextMenu();
                }
                break;
        }
    }

    private void ShowContextMenu() {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Create event"), false, CreateEvent, 1);
        menu.ShowAsContext();
    }

    private Vector2Int GetCurrentMouseTiles() {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Plane plane = new Plane(
            MapEvent2D.TileToWorld(Vector2Int.zero), 
            MapEvent2D.TileToWorld(new Vector2Int(1, 1)), 
            MapEvent2D.TileToWorld(new Vector2Int(0, 1)));
        float t = 0.0f;
        if (!plane.Raycast(ray, out t)) {
            return new Vector2Int(-1, -1);
        } else {
            Vector3 intersect = ray.origin + ray.direction * t;
            return MapEvent2D.WorldToTile(intersect) + new Vector2Int(-1, -1);
        }
    }

    private bool IsLegal(Vector2Int tiles) {
        Map map = ((ObjectLayer)target).parent;
        return tiles.x >= 0 && tiles.y >= 0 && tiles.x < map.size.x && tiles.y < map.size.y;
    }

    private void CreateEvent(object data) {
        Map map = ((ObjectLayer)target).parent;
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GenericPrefabPath);
        MapEvent2D mapEvent = Instantiate(prefab).GetComponent<MapEvent2D>();
        mapEvent.name = "Event" + Random.Range(1000000, 9999999);
        GameObjectUtility.SetParentAndAlign(mapEvent.gameObject, map.objectLayer.gameObject);
        mapEvent.SetLocation(lastMouseTiles);
        Selection.activeObject = mapEvent.gameObject;
        Undo.RegisterCreatedObjectUndo(mapEvent, "Create " + mapEvent.name);
    }
}
