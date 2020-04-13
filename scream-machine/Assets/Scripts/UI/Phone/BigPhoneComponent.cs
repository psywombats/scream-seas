using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class BigPhoneComponent : PhoneComponent {

    [SerializeField] MessageDisplayComponent toDisplay = null;
    [SerializeField] MessageDisplayComponent fromDisplay = null;
    [Space]
    [SerializeField] GameObject messageSelectMode = null;
    [SerializeField] GameObject toMode = null;
    [SerializeField] GameObject fromMode = null;
    [Space]
    [SerializeField] private ListView ConversationList = null;
    [SerializeField] private GenericSelector ConversationSelector = null;

    public void Populate(Messenger messenger) {
        this.messenger = messenger;

        if (messageSelectMode.activeInHierarchy) {
            UpdateSelector();
        }
        if (toMode.activeInHierarchy && messenger.ActiveConvo != null) {
            toDisplay.Populate(messenger.ActiveConvo.LastMessage, useToMode:true);
        }
        if (fromMode.activeInHierarchy && messenger.ActiveConvo != null) {
            fromDisplay.Populate(messenger.ActiveConvo.LastMessage, useToMode:false);
        }
    }

    public override void UpdateFromMessenger(Messenger messenger) {
        base.UpdateFromMessenger(messenger);
        if (messenger == this.messenger) {
            Populate(messenger);
        }
    }

    public async Task DoMenu() {
        Populate(messenger);
        ConversationSelector.Selection = 0;
        while (true) {
            var index = await ConversationSelector.SelectItemAsync(null, true);
            if (index < 0) {
                return;
            }

            var convo = ConversationList.GetCell(index).GetComponent<ConversationCell>().Convo;
            messenger.ActiveConvo = convo;
            if (convo.PendingScript != null) {
                await convo.PlayScriptRoutine();
            } else {
                var msg = convo.LastMessage;
                await PlayMessageRoutine(msg);
                UpdateFromMessenger(messenger);
            }
            SwitchToSelectMode();
        }
    }

    private void SwitchToFromMode() {
        messageSelectMode.SetActive(false);
        toMode.SetActive(false);
        fromMode.SetActive(true);
    }

    private void SwitchToToMode() {
        messageSelectMode.SetActive(false);
        toMode.SetActive(true);
        fromMode.SetActive(false);
    }

    private void SwitchToSelectMode() {
        messageSelectMode.SetActive(true);
        toMode.SetActive(false);
        fromMode.SetActive(false);

        UpdateSelector();
    }

    private void UpdateSelector() {
        var convos = messenger.GetRecentConversations();
        ConversationList.Populate(convos.GetRange(0, Mathf.Min(6, convos.Count)), (obj, data) => {
            obj.GetComponent<ConversationCell>().Populate(data);
        });
    }

    public IEnumerator PlayMessageRoutine(string sender, string text) {
        var convo = messenger.ActiveConvo;
        var message = new Message(convo, sender == "YOU" ? messenger.Me : convo.Client, text);
        convo.AddMessage(message);
        yield return PlayMessageRoutine(message);
    }

    public IEnumerator PlayMessageRoutine(Message message) {
        if (message.Client != messenger.Me) {
            SwitchToFromMode();
            UpdateFromMessenger(messenger);
        } else {
            SwitchToToMode();
            UpdateFromMessenger(messenger);
        }
        yield return Global.Instance().Input.ConfirmRoutine();
    }
}
