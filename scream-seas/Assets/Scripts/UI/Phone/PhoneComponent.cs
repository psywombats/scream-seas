using UnityEngine;

public class PhoneComponent : MonoBehaviour {

    protected Messenger messenger;

    public void Start() {
        if (messenger == null) {
            messenger = Global.Instance().Messenger;
        }
    }

    public virtual void UpdateFromMessenger(Messenger messenger) {

    }

    public void SetMessenger(Messenger newMessenger) {
        messenger = newMessenger;
        newMessenger.UpdateFromMessenger();
    }
}
