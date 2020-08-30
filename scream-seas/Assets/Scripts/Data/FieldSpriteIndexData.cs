using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "FieldSpriteDataIndexData", menuName = "Data/Index/FieldSpriteData")]
public class FieldSpriteIndexData : GenericIndex<FieldSpriteData> {

}

[Serializable]
public class FieldSpriteData : GenericDataObject {

    public Texture2D spriteSheet;
    public bool autoAnimate = true;

    public IEnumerable<Sprite> LoadSprites() {
        var path = ResourcePathForFieldSprite(spriteSheet);
        return Resources.LoadAll<Sprite>(path);
    }

    public static string ResourcePathForFieldSprite(Texture2D spritesheet) {
        return "Sprites/Charas/" + spritesheet.name;
    }
}
