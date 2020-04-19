using UnityEngine;
using System.Collections;

public class DirSlave : MonoBehaviour {

    public FieldSpritesheetComponent field;
    public CharaEvent chara;
    public SpriteRenderer render;

    public void Update() {
        render.sprite = chara.SpriteForMain(field);
    }
}
