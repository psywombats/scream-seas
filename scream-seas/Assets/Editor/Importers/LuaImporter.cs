using UnityEngine;

using System.IO;

[UnityEditor.AssetImporters.ScriptedImporter(1, new string[] { "scene", "lua" })]
public class LuaImporter : UnityEditor.AssetImporters.ScriptedImporter {

    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext context) {
        var script = ScriptableObject.CreateInstance<LuaSerializedScript>();
        var text = File.ReadAllText(context.assetPath);
        script.luaString = text;
        context.AddObjectToAsset("Script", script);
        context.SetMainObject(script);
    }
}
