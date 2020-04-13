using UnityEngine;

public class SmsScript : ScriptableObject {

    public string script;
    public string clientTag;
    public string previewTag;
    public string previewMessage;
    public int unreadCount = 1;
    public bool playerSendsFirstMessage = false;
    
    public void InferPreview() {
        var firstLine = script.Split('\n')[0];
        if (firstLine.StartsWith("message")) {
            previewTag = firstLine.Substring(0, firstLine.IndexOf(','));
            previewTag = previewTag.Replace("\"", "");
            previewTag = previewTag.Trim();
            clientTag = previewTag;
            var start = firstLine.IndexOf("\"");
            var run = firstLine.LastIndexOf("\"") - start;
            previewMessage = firstLine.Substring(start, run);
            unreadCount = 1;
        } else {
            unreadCount = 0;
            playerSendsFirstMessage = true;
        }
    }
}
