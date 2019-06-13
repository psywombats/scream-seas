using UnityEngine;
using System.Collections;

public class BattleUI : MonoBehaviour {

    private const string InstancePath = "Prefabs/UI/BattleUI";

    public MainActionSelector mainActionSelector;
    public SkillSelector skillSelector;

    public static BattleUI Spawn() {
        GameObject obj = Instantiate(Resources.Load<GameObject>(InstancePath));
        return obj.GetComponent<BattleUI>();
    }
}
