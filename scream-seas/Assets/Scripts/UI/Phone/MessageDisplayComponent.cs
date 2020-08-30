using UnityEngine;
using UnityEngine.UI;

public class MessageDisplayComponent : MonoBehaviour {

    [SerializeField] private Text text = null;
    [SerializeField] private Text statusText = null;

    public void Populate(Message message, bool useToMode) {
        text.text = message.Text;
        statusText.text = (useToMode ? "To: " : "From: ") + message.Conversation.Client.displayName;
    }
}
