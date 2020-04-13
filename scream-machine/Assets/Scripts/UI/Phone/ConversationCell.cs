using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(SelectableCell))]
public class ConversationCell : MonoBehaviour {

    [SerializeField] private Text fromText = null;
    [SerializeField] private Text subjText = null;
    [SerializeField] private List<GameObject> unreadState = null;
    [SerializeField] private ColorPulseComponent activeState = null;

    public Conversation Convo { get; set; }

    public void Populate(Conversation convo) {
        Convo = convo;
        fromText.text = Convo.Client.displayName;
        subjText.text = Convo.GetPreviewMessageText();
        foreach (var obj in unreadState) {
            obj.SetActive(Convo.UnreadCount > 0);
        }
        activeState.Active = Convo.HasScriptAvailable;
    }
}
