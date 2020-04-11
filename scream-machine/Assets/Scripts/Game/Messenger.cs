using UnityEngine;
using System.Collections.Generic;

public class Messenger : MonoBehaviour {

    private const string ConversationsDirectory = "Conversations/";

    private List<Conversation> conversations;
    private Dictionary<string, Client> clients;

    public Messenger() {
        conversations = new List<Conversation>();
    }

    public Client GetClient(string tag) {
        if (!clients.ContainsKey(tag)) {
            clients[tag] = new Client(tag);
        }
        return clients[tag];
    }

    /// <summary>Simulates getting a new message, triggering the conversation with the given name</summary>
    public void LoadConversation(string sender, string filename) {
        var asset = Resources.Load<TextAsset>(ConversationsDirectory + sender + "/" + filename);
        Conversation convo = null;
        foreach (var possible in conversations) {
            if (possible.Client.ClientName == sender) {
                convo = possible;
                break;
            }
        }
        if (convo == null) {
            convo = new Conversation(new Client(sender));
            conversations.Add(convo);
        }

        convo.UpdateWithFile(asset.text);
    }
}
