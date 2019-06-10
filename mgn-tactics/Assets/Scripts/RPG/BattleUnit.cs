using System.Collections;
using UnityEngine;

/**
 * A unit in battle. This class should be as dumb as possible - just a unit referene with a few
 * extra fields and getters.
 */
public class BattleUnit {

    // convenience functions
    public Unit unit { get; private set; }
    public Battle battle { get; private set; }
    public BattleController controller { get { return battle.controller; } }
    public Alignment align { get { return unit.align; } }
    public BattleEvent battler { get {  return battle.controller.GetDollForUnit(this); } }
    public Vector2Int position {  get { return battler.GetComponent<MapEvent>().position; } }

    // owned information
    public int nextTurnAt { get; private set; }
    public int stepsMovedThisTurn { get; set; }
    public bool hasActedThisTurn { get; set; }
    public Vector2Int posAtStartThisTurn { get; set; }
    public bool waitingThisTurn { get; set; }

    // === INITIALIZATION ==========================================================================

    // we create battle units from:
    //  - unit, this is a keyed by what comes in from tiled and used to look up hero/enemy in db
    //  - battle, the parent battle creating this unit for
    public BattleUnit(Unit unit, Battle battle) {
        this.unit = unit;
        this.battle = battle;
    }

    // === RPG ACCESSORS ===========================================================================

    public float Get(StatTag tag) {
        return unit.stats.Get(tag);
    }

    public bool Is(StatTag tag) {
        return unit.stats.Is(tag);
    }

    // checks for deadness and dead-like conditions like petrification
    public bool IsDead() {
        return unit.IsDead();
    }

    // after spending energy on a turn, mark that here
    public void AddTurnDelay(int timeUnits) {
        nextTurnAt = battle.NextFreeDelay(nextTurnAt + timeUnits);
    }

    // === PER TURN LOGIC ==========================================================================

    public bool CanMoveMoreThisTurn() {
        return stepsMovedThisTurn == 0;
    }

    public bool CanActThisTurn() {
        return !hasActedThisTurn;
    }

    public bool CanDoAnythingThisTurn() {
        return !waitingThisTurn && (CanActThisTurn() || CanMoveMoreThisTurn());
    }
}
