using UnityEngine;
using System.Collections;

public class SwitchEnabler : MonoBehaviour {

    [SerializeField] private string switchName = "";
    [SerializeField] private GameObject toEnable = null;
    [SerializeField] private GameObject toDisable = null;

    private bool switched;

    public void OnEnable() {
        switched = CheckCondition();
        CheckSwitch();
    }

    public void Update() {
        CheckSwitch();
    }

    private void CheckSwitch() {
        if (CheckCondition() != switched) {
            switched = CheckCondition();
            if (toEnable != null) toEnable.SetActive(enabled);
            if (toDisable != null) toDisable.SetActive(!enabled);
        }
    }

    private bool CheckCondition() {
        return Global.Instance().Data.GetSwitch(switchName);
    }
}
