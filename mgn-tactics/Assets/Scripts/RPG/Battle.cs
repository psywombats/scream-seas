using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A battle in progress. Responsible for all battle logic, state, and control flow. The actual
 * battle visual representation is contained in the BattleController, this should just be the logic.
 * 
 * Flow for battles works like this:
 *  - The scene transitions to the tactics scene
 *  - The relevant tactics is loaded into the scene
 *  - A battle controller exists on the loaded map
 *  - A new battle is created and passed to this controller
 *  - The controller takes control
 */
public class Battle {

    public AIController ai { get; private set; }
    
    public BattleController controller { get; private set; }
    
    private List<BattleUnit> units;

    // === INITIALIZATION ==========================================================================

    public Battle() {
        units = new List<BattleUnit>();
        ai = new AIController(this);
    }

    // === BOOKKEEPING AND GETTERS =================================================================

    public ICollection<BattleUnit> AllUnits() {
        return units;
    }

    public IEnumerable<BattleUnit> UnitsByAlignment(Alignment align) {
        return units.Where(unit => unit.align == align);
    }

    public IEnumerable<BattleUnit> LivingUnits() {
        return units.Where(unit => !unit.IsDead());
    }

    // if we see someone with Malu's unit, we should add the Malu instance, eg
    public BattleUnit AddUnitFromSerializedUnit(Unit unit, Vector2Int startingLocation) { 
        Unit instance = Global.Instance().Party.LookUpUnit(unit.name);
        BattleUnit battleUnit = new BattleUnit(instance, this);
        battleUnit.AddTurnDelay(0);
        AddUnit(battleUnit);

        return battleUnit;
    }

    private void AddUnit(BattleUnit unit) {
        units.Add(unit);
    }

    // === TURN LOGIC ==============================================================================

    // returns which alignment won the game, or Alignment.None if no one did
    private Alignment CheckGameOver() {
        bool enemySeen = false;
        bool playerSeen = false;
        foreach (BattleUnit unit in units) {
            if (unit.IsDead()) {
                continue;
            }
            switch (unit.align) {
                case Alignment.Enemy:
                    enemySeen = true;
                    break;
                case Alignment.Hero:
                    playerSeen = true;
                    break;
            }
        }
        if (!enemySeen) {
            return Alignment.Hero;
        } else if (!playerSeen) {
            return Alignment.Enemy;
        } else {
            return Alignment.None;
        }
    }

    // units aren't allowed to have overlapping NextTurnAt, so when setting a new one, always use
    // this function to resolve conflicts
    public int NextFreeDelay(int requestedPosition) {
        foreach (BattleUnit unit in units) {
            if (unit.nextTurnAt == requestedPosition) {
                return NextFreeDelay(requestedPosition + 1);
            }
        }
        return requestedPosition;
    }

    public BattleUnit GetNextActor() {
        int minTurn = 0;
        BattleUnit nextUnit = null;
        foreach (BattleUnit unit in LivingUnits()) {
            if (minTurn == 0 || unit.nextTurnAt < minTurn) {
                minTurn = unit.nextTurnAt;
                nextUnit = unit;
            }
        }
        return nextUnit;
    }

    // === STATE MACHINE ===========================================================================

    // runs and executes this battle
    public IEnumerator BattleRoutine(BattleController controller) {
        this.controller = controller;
        while (true) {
            yield return NextActionRoutine();
            if (CheckGameOver() != Alignment.None) {
                yield break;
            }
        }
    }

    private IEnumerator NextActionRoutine() {
        BattleUnit actor = GetNextActor();
        yield return controller.OnUnitTurnRoutine(actor);

        if (actor.align == Alignment.Enemy) {
            yield return ai.PlayNextEnemyAction(actor);
        } else {
            Result<MainActionType> mainResult = new Result<MainActionType>();
            yield return controller.SelectMainAction(mainResult, actor);
        }

        yield return null;

        //// TODO: remove this nonsense
        //Result<Unit> targetedResult = new Result<Unit>();
        //yield return controller.SelectAdjacentUnitRoutine(targetedResult, actingUnit, (Unit unit) => {
        //    return unit.align == Alignment.Enemy;
        //});
        //if (targetedResult.canceled) {
        //    // TODO: reset where they came from
        //    yield return PlayNextHumanActionRoutine();
        //    yield break;
        //}
        //Unit targetUnit = targetedResult.value;
        //targetUnit.doll.GetComponent<CharaEvent>().FaceToward(actingUnit.doll.GetComponent<MapEvent>());

        //yield return Global.Instance().Maps.activeDuelMap.EnterMapRoutine(actingUnit.doll, targetUnit.doll);
        //yield return Global.Instance().Maps.activeDuelMap.ExitMapRoutine();

    }
}
