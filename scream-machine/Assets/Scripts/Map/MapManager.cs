using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MapManager : MonoBehaviour {

    public const string EventTeleport = "teleport";

    private string activeMapName;

    public Map ActiveMap { get; private set; }
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

    public IEnumerator NewGameRoutine(FadeImageEffect fade) {
        TransitionData data = IndexDatabase.Instance().Transitions.GetData("fade_long");
        Global.Instance().Messenger.SetNextScript("y!sms/1_01", true);
        Global.Instance().Messenger.SetNextScript("bell/bell", true);
        Global.Instance().Data.SetSwitch("nighttime", true);
        MapOverlayUI.Instance().pcSystem.SetNewsModel(IndexDatabase.Instance().PCNews.GetData("day1"));
        yield return SceneManager.LoadSceneAsync("Map2D");
        Global.Instance().Maps.RawTeleport("Apartment/Bedroom", "start", OrthoDir.East);
        Avatar.PauseInput();
        var fadeImage = Camera.GetComponent<FadeImageEffect>();
        yield return fadeImage.FadeRoutine(IndexDatabase.Instance().Fades.GetData(data.FadeOutTag), false, 0.0f);
        yield return fadeImage.FadeRoutine(IndexDatabase.Instance().Fades.GetData(data.FadeInTag), true, 3.0f);
        yield return CoUtils.RunTween(MapOverlayUI.Instance().miniPhone.transform.DOLocalMoveY(0.0f, 1.0f));
        Avatar.UnpauseInput();
    }

    public IEnumerator TeleportRoutine(string mapName, Vector2Int location, OrthoDir? facing = null, bool isRaw = false) {
        Avatar?.PauseInput();
        TransitionData data = IndexDatabase.Instance().Transitions.GetData(FadeComponent.DefaultTransitionTag);
        var mult = Global.Instance().Data.GetSwitch("long_fade") ? 3 : 1;
        if (Global.Instance().Data.GetSwitch("long_fade")) {
            StartCoroutine(Global.Instance().Audio.FadeOutRoutine(1.0f));
        }
        if (!isRaw) {
            var fadeIn = IndexDatabase.Instance().Fades.GetData(data.FadeInTag);
            yield return Camera.GetComponent<FadeImageEffect>().FadeRoutine(fadeIn, false, mult);
            RawTeleport(mapName, location, facing);
            var fadeOut = IndexDatabase.Instance().Fades.GetData(data.FadeOutTag);
            yield return Camera.GetComponent<FadeImageEffect>().FadeRoutine(fadeOut, true, mult);
        } else {
            RawTeleport(mapName, location, facing);
        }
        Avatar.UnpauseInput();
    }

    public IEnumerator TeleportRoutine(string mapName, string targetEventName, OrthoDir? facing = null, bool isRaw = false) {
        bool avatarExists = Avatar != null;
        if (avatarExists) Avatar.PauseInput();
        TransitionData data = IndexDatabase.Instance().Transitions.GetData(FadeComponent.DefaultTransitionTag);
        var mult = Global.Instance().Data.GetSwitch("long_fade") ? 3 : 1;
        if (Global.Instance().Data.GetSwitch("long_fade")) {
            StartCoroutine(Global.Instance().Audio.FadeOutRoutine(1.0f));
        }
        if (!isRaw) {
            var fadeIn = IndexDatabase.Instance().Fades.GetData(data.FadeInTag);
            yield return Camera.GetComponent<FadeImageEffect>().FadeRoutine(fadeIn, false, mult);
            RawTeleport(mapName, targetEventName, facing);
            var fadeOut = IndexDatabase.Instance().Fades.GetData(data.FadeOutTag);
            yield return Camera.GetComponent<FadeImageEffect>().FadeRoutine(fadeOut, true, mult);
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
                if (ActiveMap.transform.parent != null) {
                    DestroyImmediate(ActiveMap.transform.parent.gameObject);
                } else {
                    DestroyImmediate(ActiveMap.gameObject);
                }                
            }

            ActiveMap = map;
            ActiveMap.OnTeleportTo();
            camera = null;
            Camera.target = Avatar.Event;
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
        if (newMapObject == null) {
            var name3 = "Raw" + Map.ResourcePath + mapName;
            newMapObject = Resources.Load<GameObject>(name3);
        }
        Assert.IsNotNull(newMapObject, "Couldn't find map " + mapName);
        var obj = Instantiate(newMapObject);
        var map = obj.GetComponent<Map>();
        foreach (Transform child in obj.transform) {
            if (map != null) break;
            map = child.gameObject.GetComponent<Map>();
        }

        Camera.GetComponent<FadeImageEffect>().SnapFade();
        map.InternalName = mapName;
        return map;
    }

    private void AddInitialAvatar(Map map) {
        Avatar = Instantiate(Resources.Load<GameObject>("Prefabs/Avatar2D")).GetComponent<AvatarEvent>();
        Avatar.transform.SetParent(map.objectLayer.transform, false);
    }
}
