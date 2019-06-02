using UnityEngine;

/**
 * A serialized lua script, because Unity can't handle .lua files for some damn reason. This right
 * now is also used as the battle animation format.
 */
[CreateAssetMenu(fileName = "Script", menuName = "LuaScript")]
public class LuaSerializedScript : AutoExpandingScriptableObject {

    [TextArea(12, 36)]
    public string luaString;

    public LuaScript ToScript(LuaContext context) {
        return new LuaScript(context, luaString);
    }
}
