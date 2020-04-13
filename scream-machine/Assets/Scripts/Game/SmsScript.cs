using UnityEngine;

public class SmsScript : ScriptableObject {

    private const string ScriptsDirectory = "SmsScript/";

    public string clientTag;
    public string previewMessage;
    public int unreadCount = 1;
    public bool PlayerSendsFirstMessage => unreadCount > 0;

    [TextArea(3, 20)] public string script;

    public static SmsScript LoadByName(string name) {
        var path = ScriptsDirectory + name;
        var script = Resources.Load<SmsScript>(path);
        if (script == null) {
            Debug.Log("Could not find script " + name);
        }
        return script;
    }
}
