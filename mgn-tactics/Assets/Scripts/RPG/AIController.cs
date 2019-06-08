using UnityEngine;
using System.Collections;

/**
 * A big bad brain that dictates how enemy units take their turns. At some point it should be set up
 * to take in a configuration file.
 */
public class AIController {

    private Battle battle;
    private BattleController controller;

    // set up internal state at the start of a battle
    public void ConfigureForBattle(Battle battle) {
        this.battle = battle;
        this.controller = battle.controller;
    }

    // called repeatedly by the battle while ai units still have moves left
    public IEnumerator PlayNextAIActionRoutine() {
        BattleUnit actor = battle.GetFaction(Alignment.Enemy).NextMoveableUnit();
        controller.TargetCameraToLocation(actor.position);
        yield return new WaitForSeconds(0.8f);

        // TODO: the ai
    }
}
