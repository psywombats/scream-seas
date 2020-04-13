using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Messenger : IComparer<Conversation> {

    [SerializeField] private Client me;

    public Conversation ActiveConvo { get; set; }

    public Client Me => me;
    public bool HasScriptAvailable => conversationsByClient.Values.Any(convo => convo.HasScriptAvailable);
    
    private LuaCutsceneContext lua;
    private Dictionary<Client, Conversation> conversationsByClient;
    private Dictionary<string, Client> clientsByTag;

    public Messenger() {
        conversationsByClient = new Dictionary<Client, Conversation>();
        clientsByTag = new Dictionary<string, Client>();

        lua = new LuaCutsceneContext();
        lua.Initialize();

        me = IndexDatabase.Instance().Clients.GetData("you");
    }

    public Client GetClient(string tag) {
        if (!clientsByTag.ContainsKey(tag)) {
            clientsByTag[tag] = new Client(IndexDatabase.Instance().Clients.GetData(tag));
        }
        return clientsByTag[tag];
    }

    public Conversation GetConversation(Client client) {
        conversationsByClient.TryGetValue(client, out Conversation convo);
        if (convo == null) {
            convo = new Conversation(client);
            conversationsByClient[client] = convo;
        }
        return convo;
    }
    public Conversation GetConversation(string clientTag) {
        return GetConversation(GetClient(clientTag));
    }

    public void UpdateFromMessenger() {
        MapOverlayUI.Instance().bigPhone.UpdateFromMessenger(this);
        MapOverlayUI.Instance().miniPhone.UpdateFromMessenger(this);
    }

    public void SetNextScript(SmsScript script, bool preread) {
        var convo = GetConversation(script.clientTag);
        convo.SetNextScript(script, preread);
        UpdateFromMessenger();
    }

    public void SetNextScript(string scriptName, bool preread, float delay = 0.0f) {
        var script = SmsScript.LoadByName(scriptName);
        Global.Instance().StartCoroutine(CoUtils.RunAfterDelay(delay, () => {
            SetNextScript(script, preread);
        }));
    }

    public IEnumerator PlayScriptRoutine(SmsScript sms) {
        var luaScript = new LuaScript(lua, sms.script);
        yield return luaScript.RunRoutine(true);
    }

    public List<Conversation> GetRecentConversations() {
        var convos = new List<Conversation>(conversationsByClient.Values);
        convos.Sort(this);
        return convos;
    }

    public Message GetMostRecentUnread() {
        var convos = GetRecentConversations();
        var convo = convos.Where(c => c.UnreadCount > 0).FirstOrDefault();
        if (convo == null) return null;
        return new Message(convo, convo.Client, convo.GetPreviewMessageText());
    }

    public int Compare(Conversation x, Conversation y) {
        if (x.UnreadCount != y.UnreadCount) {
            return x.UnreadCount > 0 ? -1 : 1;
        }
        if (x.HasScriptAvailable != y.HasScriptAvailable) {
            return x.HasScriptAvailable ? -1 : 1;
        }
        return -x.ModifiedTime.CompareTo(y.ModifiedTime);
    }
}
