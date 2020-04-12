using UnityEngine;
using System.Collections.Generic;

public class Conversation : MonoBehaviour {

    public Client Client { get; private set; }
    public List<Message> Messages { get; private set; }

    public SmsScript PendingConversation { get; private set; }
    public int UnreadCount => PendingConversation == null ? 0 : PendingConversation.unreadCount;

    public Conversation(Client client) {
        Client = client;
        Messages = new List<Message>();
    }
    
    public void UpdateWithScript(SmsScript script) {
        PendingConversation = script;
    }
}
