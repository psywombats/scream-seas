using UnityEngine;
using System.Collections.Generic;

public class Conversation : MonoBehaviour {

    public Client Client { get; private set; }
    public List<Message> Messages { get; private set; }

    private List<Message> unreadMessages;

    public Conversation(Client client) {
        Client = client;

        Messages = new List<Message>();
        unreadMessages = new List<Message>();
    }

    public void AddMessage(Message message) {
        Messages.Add(message);
    }

    /// <summary>Add unread messages from a text source</summary>
    public void UpdateWithFile(string filedump) {
        var lines = filedump.Split();
        foreach (var line in lines) {
            var senderTag = line.Substring(0, line.IndexOf(':'));
            var sender = Global.Instance().Messenger.GetClient(senderTag);
            var text = line.Substring(line.IndexOf(':') + 1);
            var message = new Message(this, sender, text);
            unreadMessages.Add(message);
        }
    }

    public int GetUnreadDisplayCount() {
        return 0;
    }
}
