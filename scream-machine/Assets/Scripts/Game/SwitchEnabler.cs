using UnityEngine;
using System.Collections;

public class SwitchEnabler : MonoBehaviour {

    [SerializeField] private string switchName = "";
    [SerializeField] private GameObject toEnable = null;
    [SerializeField] private GameObject toDisable = null;
    [SerializeField] private bool invert = false;

    private bool switched;

    public void OnEnable() {
        DoUpdate();
    }

    public void Update() {
        CheckSwitch();
    }

    private void CheckSwitch() {
        if (CheckCondition() != switched) {
            DoUpdate();
        }
    }

    private bool CheckCondition() {
        return Global.Instance().Data.GetSwitch(switchName);
    }

    private void DoUpdate() {
        switched = CheckCondition();
        if (toEnable != null) toEnable.SetActive(switched ^ invert);
        if (toDisable != null) toDisable.SetActive(!switched ^ invert);
    }
}
