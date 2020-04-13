using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

[ScriptedImporter(1, new string[] { "sms" })]
public class SmsImporter : ScriptedImporter {

    public override void OnImportAsset(AssetImportContext context) {
        var sms = ScriptableObject.CreateInstance<SmsScript>();
        var text = File.ReadAllText(context.assetPath);
        var lines = text.Split('\n');

        sms.script = text;
        sms.unreadCount = 0;
        for (int i = 0; i < lines.Length; i += 1) {
            if (lines[i].StartsWith("message(")) {
                int start, run;
                start = lines[i].IndexOf('\"') + 1;
                run = lines[i].IndexOf(',') - (start + 1);
                var tag = lines[i].Substring(start, run);
                if (tag == "YOU") {
                    return;
                } else {
                    sms.clientTag = tag;
                    sms.unreadCount += 1;
                    if (sms.previewMessage == null) {
                        start = lines[i].IndexOf(',') + 3; //, "
                        run = lines[i].LastIndexOf('\"') - start;
                        sms.previewMessage = lines[i].Substring(start, run);
                    }
                    break;
                }
            }
        }

        context.AddObjectToAsset("Script", sms);
        context.SetMainObject(sms);
    }
}
