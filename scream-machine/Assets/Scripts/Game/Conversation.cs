using UnityEngine;
using System;
using System.Collections;

public class Conversation {

    public Client Client { get; private set; }

    public Message LastMessage { get; private set; }
    public SmsScript PendingScript { get; private set; }
    public DateTime ModifiedTime { get; private set; }
    public int UnreadCount => !HasScriptAvailable ? 0 : PendingScript.unreadCount;
    public bool HasScriptAvailable => PendingScript != null && !preread;

    private bool preread;

    public Conversation(Client client) {
        Client = client;
    }

    public string GetPreviewMessageText() {
        if (PendingScript != null) {
            if (PendingScript.previewMessage != null && PendingScript.previewMessage.Length > 0) {
                return PendingScript.previewMessage;
            }
        }
        return LastMessage.Text;
    }
    
    public void SetNextScript(SmsScript script, bool preread) {
        this.preread = preread;
        ModifiedTime = DateTime.UtcNow;
        PendingScript = script;
    }

    public void AddMessage(Message message) {
        LastMessage = message;
        Global.Instance().Messenger.UpdateFromMessenger();
        ModifiedTime = DateTime.UtcNow;
    }

    public IEnumerator PlayScriptRoutine() {
        yield return Global.Instance().Messenger.PlayScriptRoutine(PendingScript);
        PendingScript = null;
        Global.Instance().Messenger.UpdateFromMessenger();
    }
}
