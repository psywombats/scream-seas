using DG.Tweening;
using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(Dispatch))]
[RequireComponent(typeof(SuperCustomProperties))]
[DisallowMultipleComponent]
public abstract class MapEvent : MonoBehaviour {

    public const string EventInteract = "interact";
    public const string EventCollide = "collide";
    public const string EventEnabled = "enabled";
    public const string EventStep = "step";

    public const string PropertyAppearance = "appearance";
    public const string PropertyPassable = "passable";

    public const string PropertyLuaHide = "hide";
    public const string PropertyLuaCollide = "onCollide";
    public const string PropertyLuaInteract = "onInteract";
    public const string PropertyLuaAutostart = "onEnter";
    public const string PropertyLuaBehavior = "onBehavior";
    
    protected int baseTilesPerSecond = 4;
    protected int TilesPerSecond => (int) (baseTilesPerSecond * SpeedMult);
    protected const float BehaviorMaxDelaySeconds = 7.0f;

    private Vector3 pixelImperfectPos;
    private int lastUpdatedFrame;
    private float toNextBehavior;
    private bool hasAuto;

    // Editor properties
    public Vector2Int Position = new Vector2Int(0, 0);
    public SuperCustomProperties Properties;
    [HideInInspector] public Vector2Int Size;

    // Properties
    public LuaMapEvent LuaObject { get; private set; }
    public Vector3 TargetPositionPx { get; set; }
    public bool Tracking { get; private set; }
    public bool ImpassabilityOverride { get; set; }
    public float SpeedMult { get; set; } = 1;

    public Vector3 PositionPx {
        get { return transform.localPosition; }
    }

    public Map Map {
        get {
            GameObject parentObject = gameObject;
            while (parentObject.transform.parent != null) {
                parentObject = parentObject.transform.parent.gameObject;
                Map map = parentObject.GetComponent<Map>();
                if (map != null) {
                    return map;
                }
            }
            return null;
        }
    }

    public ObjectLayer Layer {
        get {
            GameObject parent = gameObject;
            while (parent.transform.parent != null) {
                parent = parent.transform.parent.gameObject;
                ObjectLayer objLayer = parent.GetComponent<ObjectLayer>();
                if (objLayer != null) {
                    return objLayer;
                }
            }
            return null;
        }
    }

    private bool switchEnabled = true;
    public bool IsSwitchEnabled {
        get { return switchEnabled; }
        set {
            if (value != switchEnabled) {
                switchEnabled = value;
                GetComponent<Dispatch>().Signal(EventEnabled, value);
            }
        }
    }

    // convert from tile coordinates to that spot in world space
    public abstract Vector3 OwnTileToWorld(Vector2Int location);
    public abstract Vector2Int OwnWorldToTile(Vector3 location);

    // if this event were to move in a direction, how would that affect coordinates?
    public abstract Vector2Int OffsetForTiles(OrthoDir dir);

    // perform any pixel-perfect rounding needed for a pixel position
    public abstract Vector3 InternalPositionToDisplayPosition(Vector3 position);

    // what's the direction from this event towards that position?
    public abstract OrthoDir DirectionTo(Vector2Int position);

    // we have a solid TileX/TileY, please move the doll to the correct screen space
    public abstract void SetScreenPositionToMatchTilePosition();
    public abstract void SetTilePositionToMatchScreenPosition();

    // set the one xyz coordinate not controlled by arrow keys
    public abstract void SetDepth();

    // where should is the internal origin of this event?
    public abstract Vector3 GetHandlePosition();

    protected abstract bool UsesSnap();

    protected abstract void DrawGizmoSelf();

    public void Awake() {
        LuaObject = new LuaMapEvent(this);
    }

    public void Start() {
        toNextBehavior = Random.Range(0.5f, 1.0f) * BehaviorMaxDelaySeconds;
        pixelImperfectPos = PositionPx;

        GenerateLua();

        GetComponent<Dispatch>().RegisterListener(EventCollide, (object payload) => {
            OnCollide((AvatarEvent)payload);
        });
        GetComponent<Dispatch>().RegisterListener(EventInteract, (object payload) => {
            OnInteract((AvatarEvent)payload);
        });

        var appearanceResult = LuaObject.Evaluate(PropertyAppearance);
        if (appearanceResult.IsNotNil() && GetComponent<CharaEvent>() != null) {
            GetComponent<CharaEvent>().SetAppearanceByTag(appearanceResult.String);
        }

        var prop = GetProperty(PropertyLuaAutostart);
        hasAuto = prop != null && prop.Length > 0;

        CheckEnabled();
    }

    public void Update() {
        if (lastUpdatedFrame != Global.Instance().Data.SwitchLastUpdatedFrame) {
            lastUpdatedFrame = Global.Instance().Data.SwitchLastUpdatedFrame;
            CheckEnabled();
        }
        CheckAutostart(IsSwitchEnabled);
        if (Tracking) {
            var resolution = Map.UnitsPerTile / Map.PxPerTile;
            var x = Mathf.Round(pixelImperfectPos.x * (1.0f / resolution)) * resolution;
            var y = Mathf.Round(pixelImperfectPos.y * (1.0f / resolution)) * resolution;
            transform.position = new Vector3(x, y, transform.position.z);
        }
        var avatar = Global.Instance().Maps.Avatar;
        if (avatar != null && !avatar.IsInputPaused) {
            toNextBehavior -= Time.deltaTime;
            CheckBehavior();
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        if (Selection.activeGameObject == gameObject) {
            Gizmos.color = Color.red;
        } else {
            Gizmos.color = Color.magenta;
        }
        DrawGizmoSelf();
    }
#endif

    public void OnValidate() {
        if (Properties == null) {
            Properties = GetComponent<SuperCustomProperties>();
        }
    }

    public void OnStepStarted() {
        CheckBehavior();
    }

    public void CheckEnabled() {
        IsSwitchEnabled = !LuaObject.EvaluateBool(PropertyLuaHide, !IsSwitchEnabled);
    }

    public bool IsPassable() {
        string passable = GetProperty(PropertyPassable);
        if (passable == "IMPASSABLE") return false;
        if (passable == "PASSABLE") return true;
        if (ImpassabilityOverride) return false;
        if (GetComponent<CharaEvent>()) return false;
        return true;
    }

    public bool IsPassableBy(MapEvent other) {
        return IsPassable() || !IsSwitchEnabled;
    }

    public OrthoDir DirectionTo(MapEvent other) {
        return DirectionTo(other.Position);
    }

    public bool CanPassAt(Vector2Int loc) {
        if (!GetComponent<MapEvent>().IsSwitchEnabled) {
            return true;
        }
        if (loc.x < 0 || loc.x >= Map.Width || loc.y < 0 || loc.y >= Map.Height) {
            return false;
        }
        if (!Map.IsChipPassableAt(loc)) {
            return false;
        }
        if (!IsPassable()) {
            foreach (MapEvent mapEvent in Map.GetEventsAt(loc)) {
                if (!mapEvent.IsPassableBy(this)) {
                    return false;
                }
            }
        }

        return true;
    }

    public string GetProperty(string propertyName) {
        if (Properties.TryGetCustomProperty(propertyName, out CustomProperty prop)) {
            var str = prop.GetValueAsString();
            //if (str.StartsWith("this.")) {
            //    str = str.Substring("this.".Length);
            //}
            return str;
        } else {
            return "";
        }
    }

    public bool ContainsPosition(Vector2Int loc) {
        Vector2Int pos1 = Position;
        Vector2Int pos2 = Position + Size;
        return loc.x >= pos1.x && loc.x < pos2.x && loc.y >= pos1.y && loc.y < pos2.y;
    }

    public void SetPosition(Vector2Int location) {
        Position = location;
        SetScreenPositionToMatchTilePosition();
        SetDepth();
    }

    public void GenerateLua() {
        LuaObject.Set(PropertyLuaHide, GetProperty(PropertyLuaHide));
        LuaObject.Set(PropertyLuaAutostart, GetProperty(PropertyLuaAutostart));
        LuaObject.Set(PropertyLuaCollide, GetProperty(PropertyLuaCollide));
        LuaObject.Set(PropertyLuaInteract, GetProperty(PropertyLuaInteract));
        LuaObject.Set(PropertyLuaBehavior, GetProperty(PropertyLuaBehavior));

        var appearance = GetProperty(PropertyAppearance);
        var prefix = "lua(";
        if (appearance.StartsWith(prefix)) {
            var luaString = appearance.Substring(prefix.Length, appearance.Length - prefix.Length - 1);
            LuaObject.Set(PropertyAppearance, luaString);
        }

    }

    private void CheckAutostart(bool enabled) {
        if (hasAuto && enabled && !Global.Instance().Maps.Avatar.IsInputPaused) {
            LuaObject.Run(PropertyLuaAutostart);
        }
    }

    private void CheckBehavior() {
        if (toNextBehavior <= 0) {
            toNextBehavior = Random.Range(0.5f, 1.0f) * BehaviorMaxDelaySeconds;
            LuaObject.Run(PropertyLuaBehavior, false);
        }
    }

    // called when the avatar stumbles into us
    // before the step if impassable, after if passable
    private void OnCollide(AvatarEvent avatar) {
        LuaObject.Run(PropertyLuaCollide);
    }

    // called when the avatar stumbles into us
    // facing us if impassable, on top of us if passable
    private void OnInteract(AvatarEvent avatar) {
        LuaObject.Run(PropertyLuaInteract);
    }

    private LuaScript ParseScript(string lua) {
        if (lua == null || lua.Length == 0) {
            return null;
        } else {
            return new LuaScript(Global.Instance().Maps.Lua, lua);
        }
    }

    private LuaCondition ParseCondition(string lua) {
        if (lua == null || lua.Length == 0) {
            return null;
        } else {
            return Global.Instance().Maps.Lua.CreateCondition(lua);
        }
    }

    public IEnumerator StepRoutine(OrthoDir dir) {
        if (Tracking) {
            yield break;
        }

        Position += OffsetForTiles(dir);

        if (GetComponent<CharaEvent>() == null) {
            yield return LinearStepRoutine(dir);
        } else {
            yield return GetComponent<CharaEvent>().StepRoutine(dir);
        }
    }

    public IEnumerator StepMultiRoutine(OrthoDir dir, int count) {
        for (int i = 0; i < count; i += 1) {
            yield return StartCoroutine(StepRoutine(dir));
        }
    }

    public IEnumerator LinearStepRoutine(OrthoDir dir) {
        Tracking = true;
        TargetPositionPx = OwnTileToWorld(Position);
        pixelImperfectPos = PositionPx;
        var tween = DOTween.To(() => pixelImperfectPos, x => pixelImperfectPos = x, TargetPositionPx, 1.0f / TilesPerSecond);
        tween.SetEase(Ease.Linear);
        yield return CoUtils.RunTween(tween);
        transform.position = TargetPositionPx;
        Tracking = false;
    }
    public IEnumerator DiagRoutine(OrthoDir dir) {
        Tracking = true;
        if (GetComponent<CharaEvent>().Facing == OrthoDir.East && dir == OrthoDir.East) {
            Position += OffsetForTiles(OrthoDir.North);
            Position += OffsetForTiles(OrthoDir.East);
        }
        if (GetComponent<CharaEvent>().Facing == OrthoDir.West && dir == OrthoDir.East) {
            Position += OffsetForTiles(OrthoDir.South);
            Position += OffsetForTiles(OrthoDir.West);
        }

        TargetPositionPx = OwnTileToWorld(Position);
        pixelImperfectPos = PositionPx;
        var tween = DOTween.To(() => pixelImperfectPos, x => pixelImperfectPos = x, TargetPositionPx, (2.0f) / TilesPerSecond);
        tween.SetEase(Ease.Linear);
        yield return CoUtils.RunTween(tween);
        transform.position = TargetPositionPx;
        Tracking = false;
    }

    public IEnumerator LadderRoutine(int count, OrthoDir dir) {
        GetComponent<CharaEvent>().Facing = OrthoDir.North;
        GetComponent<CharaEvent>().FixFace();
        baseTilesPerSecond /= 2;
        for (int i = 0; i < count; i += 1) {
            yield return StepRoutine(dir);
        }
        baseTilesPerSecond *= 2;
        GetComponent<CharaEvent>().CancelFix();
    }

    public IEnumerator PathToRoutine(Vector2Int location, bool ignoreEvents = false) {
        List<Vector2Int> path = Map.FindPath(this, location, ignoreEvents);
        if (path == null) {
            yield break;
        }
        foreach (Vector2Int target in path) {
            OrthoDir dir = DirectionTo(target);
            yield return StepRoutine(dir);
        }
    }
}
