using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Responsible for user input and rendering during a battle. Control flow is actually handled by
 * the Battle class. This should mostly a collection of ui routines and stuff that the battle needs
 * to interact with the physical environment.
 */
[RequireComponent(typeof(Map))]
public class BattleController : MonoBehaviour {

    private const string ListenerId = "BattleControllerListenerId";

    // interally populated
    public Battle battle { get; private set; }
    public FreeCursor cursor { get; private set; }
    public DirectionCursor dirCursor { get; private set; }
    public BattleUI ui { get; private set; }

    // internal state
    private Dictionary<BattleUnit, BattleEvent> dolls;

    // convenience getters
    public Map map { get { return GetComponent<Map>(); } }
    public TacticsCam cam { get { return TacticsCam.Instance(); } }

    // === INITIALIZATION ==========================================================================

    public void Start() {
        // TODO: create this upon scene loading
        battle = new Battle();
        dolls = new Dictionary<BattleUnit, BattleEvent>();

        AddUnitsFromMap();

        cursor = FreeCursor.GetInstance();
        cursor.gameObject.transform.SetParent(GetComponent<Map>().objectLayer.transform);
        cursor.gameObject.SetActive(false);

        dirCursor = DirectionCursor.GetInstance();
        dirCursor.gameObject.transform.SetParent(GetComponent<Map>().objectLayer.transform);
        dirCursor.gameObject.SetActive(false);

        ui = FindObjectOfType<BattleUI>();
        if (ui == null) {
            ui = BattleUI.Spawn();
        }
    }

    private void AddUnitsFromMap() {
        foreach (BattleEvent battler in map.GetEvents<BattleEvent>()) {
            BattleUnit unit = battle.AddUnitFromSerializedUnit(battler.unitData, 
                battler.GetComponent<MapEvent>().position);
            battler.Setup(this, unit);
            dolls[unit] = battler;
        }
    }

    // === GETTERS AND BOOKKEEPING =================================================================

    public BattleEvent GetDollForUnit(BattleUnit unit) {
        return dolls[unit];
    }

    public BattleUnit GetUnitAt(Vector2Int position) {
        foreach (MapEvent mapEvent in map.GetEventsAt(position)) {
            if (mapEvent.GetComponent<BattleEvent>() != null) {
                return mapEvent.GetComponent<BattleEvent>().unit;
            }
        }
        return null;
    }

    public bool PositionMeetsTargetingReq(BattleUnit actor, TargetingRequirementType req, Vector2Int pos) {
        BattleEvent battler = map.GetEventAt<BattleEvent>(pos);
        if (battler == null) {
            return req == TargetingRequirementType.NoRequirement || req == TargetingRequirementType.RequireEmpty;
        } else {
            return battler.unit.MeetsTargeterReq(actor, req);
        }
    }

    // === ANIMATION ROUTINES ======================================================================

    public IEnumerator OnUnitTurnRoutine(BattleUnit unit) {
        MoveHighlightCursorToUnit(unit);
        yield return cam.CenterCameraRoutine(unit.position, map.terrain.HeightAt(unit.position));
    }

    // === STATE MACHINE ===========================================================================

    public IEnumerator ViewMapRoutine(BattleUnit currentActor) {
        SpawnCursor(currentActor.position);
        Result<Vector2Int> unitResult = new Result<Vector2Int>();
        while (!unitResult.canceled) {
            yield return cursor.AwaitSelectionRoutine(unitResult, GenericScanner());
            if (!unitResult.canceled) {
                // TODO: display UI for this location
                unitResult.Reset();
            }
        }
        cursor.Disable();
    }

    public IEnumerator SelectMainActionRoutine(Result<MainActionType> result, List<MainActionType> allowed, BattleUnit actor) {
        MoveHighlightCursorToUnit(actor);
        yield return ui.mainActionSelector.SelectMainActionRoutine(result, allowed);
    }

    public IEnumerator SelectSkillRoutine(Result<Skill> result, BattleUnit actor) {
        yield return ui.skillSelector.SelectSkillRoutine(result, actor);
    }

    public IEnumerator ClearAllMenus() {
        yield return CoUtils.RunParallel(new IEnumerator[] {
            ui.mainActionSelector.GetComponent<ListSelector>().ShowHideRoutine(true),
            ui.skillSelector.GetComponent<ListSelector>().ShowHideRoutine(true),
        }, this);
    }

    // === GAMEBOARD AND GRAPHICAL INTERACTION =====================================================

    public FreeCursor SpawnCursor(Vector2Int position) {
        cursor.Enable(position);
        return cursor;
    }

    public DirectionCursor SpawnDirCursor(Vector2Int position) {
        dirCursor.Enable(position);
        return dirCursor;
    }

    public void TargetCameraToLocation(Vector2Int loc) {
        cam.SetTargetLocation(loc, map.terrain.HeightAt(loc));
    }

    public SelectionGrid SpawnSelectionGrid() {
        SelectionGrid grid = SelectionGrid.GetInstance();
        grid.gameObject.transform.SetParent(GetComponent<Map>().transform);
        return grid;
    }

    public void MoveHighlightCursorToUnit(BattleUnit unit) {
        SpawnCursor(unit.position);
        cursor.DisableReticules();
    }

    // === SCANNERS ================================================================================

    public Scanner GenericScanner() {
        return new Scanner((Vector2Int pos) => {
            Debug.Log("Scanning at " + pos);
        },
        () => {
        });
    }
}
