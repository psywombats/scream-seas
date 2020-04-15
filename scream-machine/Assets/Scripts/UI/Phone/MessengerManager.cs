using System.Collections;
using UnityEngine;

public class MessengerManager : MonoBehaviour {

    public Messenger Current { get; set; }

    public IEnumerator ForeignPhoneRoutine(string key) {
        var phoneData = IndexDatabase.Instance().ForeignPhones.GetData(key);
        var originalMessenger = Current;
        Current = new Messenger();
        foreach (var script in phoneData.toRun) {
            Current.SetNextScript(script, true);
        }
        MapOverlayUI.Instance().foreignPhone.SetMessenger(Current);
        yield return MapOverlayUI.Instance().phoneSystem.FlipForeignRoutine();
        yield return CoUtils.TaskAsRoutine(MapOverlayUI.Instance().foreignPhone.DoMenu());
        yield return MapOverlayUI.Instance().phoneSystem.FlipForeignRoutine();
        Current = originalMessenger;
    }
}
