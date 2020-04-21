using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * For our purposes, a CharaEvent is anything that's going to be moving around the map
 * or has a physical appearance. For parallel process or whatevers, they won't have this.
 */
[DisallowMultipleComponent]
[RequireComponent(typeof(FieldSpritesheetComponent))]
public class CharaEvent : MonoBehaviour {
    
    private const float DesaturationDuration = 0.5f;
    private const float StepsPerSecond = 4.0f;

    public Doll Doll;

    [SerializeField] private bool alwaysAnimates = true;
    
    private Vector2 lastPosition;
    private bool wasSteppingLastFrame;
    private Vector3 targetPx;
    private float moveTime;
    private bool stepping;
    private bool faceFix;

    public MapEvent Parent { get { return GetComponent<MapEvent>(); } }
    public Map Map { get { return Parent.Map; } }
    public int StepCount => sprites.StepCount;

    private FieldSpritesheetComponent sprites;
    public FieldSpritesheetComponent Sprites {
        get {
            if (sprites == null) {
                sprites = GetComponent<FieldSpritesheetComponent>();
            }
            return sprites;
        }
    }

    [SerializeField] [HideInInspector] private OrthoDir _facing = OrthoDir.South;
    public OrthoDir Facing {
        get { return _facing; }
        set {
            if (faceFix) return;
            _facing = value;
            UpdateAppearance();
        }
    }

    public List<SpriteRenderer> Renderers;

    public void Start() {
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
            UpdateEnabled((bool)payload);
        });
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventInteract, (object payload) => {
            Facing = Parent.DirectionTo(Global.Instance().Maps.Avatar.GetComponent<MapEvent>());
        });
        UpdateEnabled(Parent.IsSwitchEnabled);
    }

    public void Update() {
        var oldX = Mathf.FloorToInt(moveTime * StepsPerSecond) % Sprites.StepCount;

        bool steppingThisFrame = IsSteppingThisFrame();
        stepping = steppingThisFrame || wasSteppingLastFrame;
        if (!steppingThisFrame && !wasSteppingLastFrame) {
            moveTime = StepsPerSecond / Sprites.StepCount;
        } else {
            moveTime += Time.deltaTime;
        }
        wasSteppingLastFrame = steppingThisFrame;
        lastPosition = transform.position;
        
            UpdateAppearance();

    }

    public void UpdateEnabled(bool enabled) {
        foreach (SpriteRenderer renderer in Renderers) {
            renderer.enabled = enabled;
        }
        UpdateAppearance();
    }

    public void UpdateAppearance() {
        if (Doll.Renderer != null) {
            Doll.Renderer.sprite = SpriteForMain();
        }
    }

    public void FaceToward(MapEvent other) {
        Facing = Parent.DirectionTo(other);
    }

    public void ResetAnimationTimer() {
        moveTime = 0.0f;
    }

    public void FixFace() => faceFix = true;
    public void CancelFix() => faceFix = false;

    public void SetTransparent(bool trans) {
        var propBlock = new MaterialPropertyBlock();
        foreach (SpriteRenderer renderer in Renderers) {
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_Trans", trans ? 1 : 0);
            renderer.SetPropertyBlock(propBlock);
        }
    }

    public void SetAppearanceByTag(string fieldSpriteTag) {
        Sprites.SetByTag(fieldSpriteTag);
        UpdateAppearance();
    }

    public IEnumerator StepRoutine(OrthoDir dir) {
        Facing = dir;
        Vector2Int offset = Parent.OffsetForTiles(dir);
        Vector3 startPx = Parent.PositionPx;
        targetPx = Parent.OwnTileToWorld(Parent.Position);
        yield return Parent.LinearStepRoutine(dir);
    }

    private bool IsSteppingThisFrame() {
        Vector2 position = transform.position;
        Vector2 delta = position - lastPosition;
        return alwaysAnimates || (delta.sqrMagnitude > 0 && delta.sqrMagnitude < Map.PxPerTile) || Parent.Tracking ||
            (GetComponent<AvatarEvent>() && GetComponent<AvatarEvent>().WantsToTrack);
    }

    public Sprite SpriteForMain(FieldSpritesheetComponent sprites = null) {
        if (sprites == null) sprites = Sprites;
        int x = Mathf.FloorToInt(moveTime * StepsPerSecond) % Sprites.StepCount;
        if (x == 3) x = 1;
        if (!stepping) x = 1;
        return sprites.FrameBySlot(x, Facing);
    }
}
