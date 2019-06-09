using UnityEngine;
using System.Collections;

public class BattleUI : MonoBehaviour {

    private const string InstancePath = "Prefabs/UI/BattleUI";

    public static BattleUI Spawn() {
        GameObject obj = Instantiate(Resources.Load<GameObject>(InstancePath));
        return obj.GetComponent<BattleUI>();
    }
}
