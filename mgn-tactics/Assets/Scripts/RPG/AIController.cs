using UnityEngine;
using System.Collections;

/**
 * A big bad brain that dictates how enemy units take their turns. At some point it should be set up
 * to take in a configuration file.
 */
public class AIController {

    public Battle battle { get; private set; }
    public BattleController controller { get { return battle.controller; } }

    public AIController(Battle battle) {
        this.battle = battle;
    }

    public IEnumerator PlayNextEnemyAction(BattleUnit unit) {
        unit.AddTurnDelay(10);
        yield return CoUtils.Wait(1.0f);
    }
}
