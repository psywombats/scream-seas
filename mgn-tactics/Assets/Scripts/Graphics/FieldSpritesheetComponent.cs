using UnityEngine;
using System.Collections.Generic;

public class FieldSpritesheetComponent : MonoBehaviour {

    [SerializeField] private Texture2D spritesheet = null;

    [SerializeField] private int stepCount = 4;
    public int StepCount => stepCount;

    public string Name => spritesheet == null ? "" : spritesheet.name;
    public bool IsSingleFrame => spritesheet != null && spritesheet.width < Map.PxPerTile * 2;
    public bool IsSingleStrip => spritesheet != null && spritesheet.height < Map.PxPerTile * 4;

    private Dictionary<string, Sprite> sprites;

    public void Awake() {
        if (spritesheet != null) {
            LoadSpritesheetData(spritesheet);
        }
    }

    public void SetByTag(string tag) {
        if (tag == null) {
            tag = "null";
        } else {
            var spritesheet = IndexDatabase.Instance().FieldSprites.GetData(tag).spriteSheet;
            LoadSpritesheetData(spritesheet);
        }
    }

    public void SetByTexture(Texture2D spritesheet) {
        LoadSpritesheetData(spritesheet);
    }

    public Sprite FrameBySlot(int x, OrthoDir facing) {
        return FrameByExplicitSlot(x, facing.Ordinal());
    }

    public Sprite FrameByExplicitSlot(int x, int y) {
        if (spritesheet == null) {
            return null;
        }
        if (sprites == null) {
            LoadSpritesheetData(spritesheet);
        }
        if (IsSingleFrame) {
            x = 0;
        }
        if (IsSingleStrip) {
            y = 0;
        }
        string name = NameForFrame(spritesheet.name, x, y);
        if (!sprites.ContainsKey(name)) {
            Debug.LogError(this + " doesn't contain frame " + name);
            return null;
        }
        return sprites[name];
    }

    public Sprite FrameForDirection(OrthoDir facing) {
        return FrameBySlot(0, facing);
    }

    private void LoadSpritesheetData(Texture2D spritesheet) {
        this.spritesheet = spritesheet;
        sprites = new Dictionary<string, Sprite>();
        var path = FieldSpriteData.ResourcePathForFieldSprite(spritesheet);
        foreach (Sprite sprite in Resources.LoadAll<Sprite>(path)) {
            sprites[sprite.name] = sprite;
        }
        stepCount = spritesheet.width / Map.PxPerTile;
    }

    public static string NameForFrame(string sheetName, int x, int y) {
        return sheetName + "_" + x + "_" + y;
    }
}
