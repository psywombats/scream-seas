using UnityEngine;
using System.Collections.Generic;

public class Messenger : MonoBehaviour {

    private const string ConversationsDirectory = "Conversations/";

    private Dictionary<Client, Conversation> conversationsByClient;
    private Dictionary<string, Client> clientsByTag;

    public Messenger() {
        conversationsByClient = new Dictionary<Client, Conversation>();
    }

    public Client GetClient(string tag) {
        if (!clientsByTag.ContainsKey(tag)) {
            clientsByTag[tag] = IndexDatabase.Instance().Clients.GetData(tag);
        }
        return clientsByTag[tag];
    }

    public Conversation GetConversation(Client client) {
        conversationsByClient.TryGetValue(client, out Conversation convo);
        return convo;
    }
    public Conversation GetConversation(string clientTag) {
        return GetConversation(GetClient(clientTag));
    }

    public void EnableScript(SmsScript script) {
        var convo = GetConversation(script.clientTag);
        convo.UpdateWithScript(script);
    }
}
