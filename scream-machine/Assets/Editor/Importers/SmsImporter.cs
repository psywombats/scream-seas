using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

[ScriptedImporter(1, new string[] { "sms" })]
public class SmsImporter : ScriptedImporter {

    public override void OnImportAsset(AssetImportContext context) {
        var sms = ScriptableObject.CreateInstance<SmsScript>();
        var text = File.ReadAllText(context.assetPath);
        sms.script = text;
        context.AddObjectToAsset("Script", sms);
        context.SetMainObject(sms);
    }
}
