using UnityEngine;
using System.Collections.Generic;

public class SwitchComponent : MonoBehaviour {

    [SerializeField] private string switchName = "";
    [SerializeField] private bool invert = false;
    [SerializeField] private List<GameObject> toToggle = null;

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
        foreach (var obj in toToggle) {
            obj.SetActive(switched ^ invert);
        }
    }
}
