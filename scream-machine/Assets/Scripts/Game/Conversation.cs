using UnityEngine;
using System;
using System.Collections;

public class Conversation {

    public Client Client { get; private set; }

    public Message LastMessage { get; private set; }
    public SmsScript PendingScript { get; private set; }
    public DateTime ModifiedTime { get; private set; }
    public int UnreadCount => forcePreview != null ? hax : (!HasScriptAvailable ? 0 : PendingScript.unreadCount);
    public bool HasScriptAvailable => PendingScript != null && !preread;

    private string forcePreview;
    private bool preread;
    private int hax = 3;

    public Conversation(Client client) {
        Client = client;
    }

    public string GetPreviewMessageText() {
        if (forcePreview != null) return forcePreview;
        if (PendingScript != null) {
            if (PendingScript.previewMessage != null && PendingScript.previewMessage.Length > 0) {
                return PendingScript.previewMessage;
            }
        }
        return LastMessage.Text;
    }
    
    public void SetNextScript(SmsScript script, bool preread) {
        this.preread = preread;
        PendingScript = script;
        if (script.unreadCount > 0) {
            ModifiedTime = DateTime.UtcNow;
        }
    }

    public void ForcePreview(string text) {
        forcePreview = text;
        ModifiedTime = DateTime.UtcNow;
        hax += 1;
        Global.Instance().Messenger.UpdateFromMessenger();
    }

    public void AddMessage(Message message) {
        LastMessage = message;
        Global.Instance().Messenger.UpdateFromMessenger();
    }

    public IEnumerator PlayScriptRoutine() {
        if (PendingScript.unreadCount > 0) {
            ModifiedTime = DateTime.UtcNow;
        }
        var script = PendingScript;
        PendingScript = null;
        yield return Global.Instance().Messenger.PlayScriptRoutine(script);
        Global.Instance().Messenger.UpdateFromMessenger();
    }
}
