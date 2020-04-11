using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MapManager : MonoBehaviour {

    public const string EventTeleport = "teleport";

    private string activeMapName;

    public Map ActiveMap { get; set; }
    public AvatarEvent Avatar { get; set; }

    private LuaContext lua;
    public LuaContext Lua {
        get {
            if (lua == null) {
                lua = new LuaCutsceneContext();
                lua.Initialize();
            }
            return lua;
        }
    }

    private new MapCamera camera;
    public MapCamera Camera {
        get {
            if (camera == null) {
                camera = FindObjectOfType<MapCamera>();
            }
            return camera;
        }
        set {
            camera = value;
        }
    }

    public void Start() {
        ActiveMap = FindObjectOfType<Map>();
        Avatar = ActiveMap?.GetComponentInChildren<AvatarEvent>();
    }

    public IEnumerator FadeOutRoutine(string tag = FadeComponent.DefaultTransitionTag) => Camera.GetComponent<FadeComponent>().FadeOutRoutine(tag);
    public IEnumerator FadeInRoutine(string tag = FadeComponent.DefaultTransitionTag) => Camera.GetComponent<FadeComponent>().FadeInRoutine(tag);

    public IEnumerator TeleportRoutine(string mapName, Vector2Int location, OrthoDir? facing = null, bool isRaw = false) {
        Avatar?.PauseInput();
        TransitionData data = IndexDatabase.Instance().Transitions.GetData(FadeComponent.DefaultTransitionTag);
        if (!isRaw) {
            yield return Camera.GetComponent<FadeComponent>().TransitionRoutine(data, () => {
                RawTeleport(mapName, location, facing);
            });
        } else {
            RawTeleport(mapName, location, facing);
        }
        Avatar.UnpauseInput();
    }

    public IEnumerator TeleportRoutine(string mapName, string targetEventName, OrthoDir? facing = null, bool isRaw = false) {
        bool avatarExists = Avatar != null;
        if (avatarExists) Avatar.PauseInput();
        TransitionData data = IndexDatabase.Instance().Transitions.GetData(FadeComponent.DefaultTransitionTag);
        if (!isRaw) {
            yield return Camera.GetComponent<FadeComponent>().TransitionRoutine(data, () => {
                RawTeleport(mapName, targetEventName, facing);
            });
        } else {
            RawTeleport(mapName, targetEventName, facing);
        }
        if (avatarExists) Avatar.UnpauseInput();
    }
    
    private void RawTeleport(string mapName, Vector2Int location, OrthoDir? facing = null) {
        Map newMapInstance = InstantiateMap(mapName);
        RawTeleport(newMapInstance, location, facing);
    }

    private void RawTeleport(string mapName, string targetEventName, OrthoDir? facing = null) {
        Map newMapInstance;
        if (mapName == activeMapName) {
            newMapInstance = ActiveMap;
        } else {
            newMapInstance = InstantiateMap(mapName);
        }
        activeMapName = mapName;
        MapEvent target = newMapInstance.GetEventNamed(targetEventName);
        RawTeleport(newMapInstance, target.Position, facing);
    }

    private void RawTeleport(Map map, Vector2Int location, OrthoDir? facing = null) {
        activeMapName = null;
        if (Avatar == null) {
            AddInitialAvatar(map);
        } else {
            Avatar.transform.SetParent(map.objectLayer.transform, false);
        }
        Avatar.GetComponent<MapEvent>().SetPosition(location);
        if (facing != null) {
            Avatar.Chara.Facing = facing.GetValueOrDefault(OrthoDir.North);
        }

        if (map != ActiveMap) {
            if (ActiveMap != null) {
                ActiveMap.OnTeleportAway();
                Destroy(ActiveMap.gameObject);
            }

            ActiveMap = map;
            ActiveMap.OnTeleportTo();
            Global.Instance().Dispatch.Signal(EventTeleport, ActiveMap);
            Avatar.OnTeleport();
        }
    }

    private Map InstantiateMap(string mapName) {
        if (mapName.EndsWith(".tmx")) {
            mapName = mapName.Substring(0, mapName.IndexOf('.'));
        }
        GameObject newMapObject = null;
        
        newMapObject = Resources.Load<GameObject>(mapName);
        if (newMapObject == null) {
            newMapObject = Resources.Load<GameObject>(mapName);
        }
        if (newMapObject == null) {
            var name2 = Map.ResourcePath + mapName;
            newMapObject = Resources.Load<GameObject>(name2);
        }
        Assert.IsNotNull(newMapObject, "Couldn't find map " + mapName);
        var map = Instantiate(newMapObject).GetComponent<Map>();
        map.InternalName = mapName;
        return map;
    }

    private void AddInitialAvatar(Map map) {
        Avatar = Instantiate(Resources.Load<GameObject>("Prefabs/Avatar2D")).GetComponent<AvatarEvent>();
        Avatar.transform.SetParent(map.objectLayer.transform, false);
        // Camera.target = Avatar.Event;
        Camera.ManualUpdate();
    }
}
