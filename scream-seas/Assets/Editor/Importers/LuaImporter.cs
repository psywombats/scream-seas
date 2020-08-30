using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

[ScriptedImporter(1, new string[] { "scene", "lua" })]
public class LuaImporter : ScriptedImporter {

    public override void OnImportAsset(AssetImportContext context) {
        var script = ScriptableObject.CreateInstance<LuaSerializedScript>();
        var text = File.ReadAllText(context.assetPath);
        script.luaString = text;
        context.AddObjectToAsset("Script", script);
        context.SetMainObject(script);
    }
}
