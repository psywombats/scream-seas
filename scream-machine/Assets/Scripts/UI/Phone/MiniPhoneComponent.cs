using UnityEngine;
using UnityEngine.UI;

public class MiniPhoneComponent : PhoneComponent {

    [SerializeField] private Text statusText = null;
    [SerializeField] private Text previewText = null;
    [SerializeField] private ColorPulseComponent activityAvailable = null;

    public override void UpdateFromMessenger(Messenger messenger) {
        base.UpdateFromMessenger(messenger);
        if (messenger == this.messenger) {
            activityAvailable.Active = messenger.HasScriptAvailable;
            var message = messenger.GetMostRecentUnread();
            if (message != null) {
                statusText.text = message.Client.displayName;
                previewText.text = message.Text;
            } else {
                statusText.text = "No new messages";
                previewText.text = "";
            }
        }
    }
}
