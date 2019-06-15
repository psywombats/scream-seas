using UnityEngine;
using System.Collections;

public class DummyAttackEffect : Effector {

    public override IEnumerator ExecuteSingleCellRoutine(SkillResult result, Vector2Int location) {
        BattleUnit victim = map.GetEventAt<BattleEvent>(location).unit;
        victim.battler.GetComponent<CharaEvent>().FaceToward(battler.GetComponent<MapEvent>());

        yield return controller.EnterDuelRoutine(actor, victim);
        yield return controller.ExitDuelRoutine();
    }
}
