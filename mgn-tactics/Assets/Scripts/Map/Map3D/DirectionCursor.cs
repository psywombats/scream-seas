using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DirectionCursor : TacticsCursor {

    private const string PrefabPath = "Prefabs/Tactics/DirectionCursor";

    public OrthoDir currentDir;
    private BattleEvent actor;
    private Result<OrthoDir> awaitingSelect;

    public static DirectionCursor GetInstance() {
        GameObject prefab = Resources.Load<GameObject>(PrefabPath);
        return Instantiate(prefab).GetComponent<DirectionCursor>();
    }

    // selects an adjacent unit to the actor (provided they meet the rule), cancelable
    public IEnumerator SelectAdjacentUnitRoutine(Result<BattleUnit> result,
                BattleUnit actingUnit,
                Func<BattleUnit, bool> rule,
                Scanner scanner = null,
                bool canCancel = true) {
        List<OrthoDir> dirs = new List<OrthoDir>();
        Map map = actingUnit.battle.controller.map;
        foreach (OrthoDir dir in Enum.GetValues(typeof(OrthoDir))) {
            Vector2Int loc = actingUnit.position + dir.XY3D();
            BattleEvent doll = map.GetEventAt<BattleEvent>(loc);
            if (doll != null && rule(doll.unit)) {
                dirs.Add(dir);
            }
        }
        if (dirs.Count > 0) {
            Result<OrthoDir> dirResult = new Result<OrthoDir>();
            yield return SelectTargetDirRoutine(dirResult, actingUnit, dirs, scanner, canCancel);
            Vector2Int loc = actingUnit.position + dirResult.value.XY3D();
            result.value = map.GetEventAt<BattleEvent>(loc).unit;
        } else {
            Debug.Assert(false, "No valid directions");
            result.Cancel();
        }
    }

    // selects a square to be targeted by the acting unit, might be canceled
    public IEnumerator SelectTargetDirRoutine(Result<OrthoDir> result,
            BattleUnit actingUnit,
            List<OrthoDir> allowedDirs,
            Scanner scanner = null,
            bool canCancel = true) {
        this.scanner = scanner;
        actor = actingUnit.battler;

        gameObject.SetActive(true);
        actingUnit.controller.cursor.DisableReticules();

        SelectionGrid grid = actingUnit.controller.SpawnSelectionGrid();
        TacticsTerrainMesh terrain = actingUnit.controller.map.terrain;
        grid.ConfigureNewGrid(actingUnit.position, 1, terrain, (Vector2Int loc) => {
            return (loc.x + loc.y + actingUnit.position.x + actingUnit.position.y) % 2 == 1;
        });
        AttemptDirection(allowedDirs[0]);

        while (!result.finished) {
            Result<OrthoDir> dirResult = new Result<OrthoDir>();
            yield return AwaitSelectionRoutine(actor, dirResult);
            if (dirResult.canceled) {
                if (canCancel) {
                    result.Cancel();
                    break;
                }
            } else {
                result.value = dirResult.value;
            }
        }

        Destroy(grid.gameObject);
        actingUnit.controller.cursor.EnableReticules();
        gameObject.SetActive(false);
    }

    public override void Enable() {
        base.Enable();
        currentDir = OrthoDir.North;
    }

    protected override void OnCancel() {
        awaitingSelect.Cancel();
    }

    protected override void OnConfirm() {
        awaitingSelect.value = currentDir;
    }

    private IEnumerator AwaitSelectionRoutine(BattleEvent actor, Result<OrthoDir> result) {
        this.actor = actor;
        awaitingSelect = result;
        while (!awaitingSelect.finished) {
            yield return null;
        }
        awaitingSelect = null;
    }

    protected override void AttemptDirection(OrthoDir dir) {
        SetDirection(dir);
    }

    private void SetDirection(OrthoDir dir) {
        currentDir = dir;
        Vector2Int pos = actor.position + dir.XY3D();
        actor.GetComponent<CharaEvent>().facing = dir;
        GetComponent<MapEvent>().SetPosition(pos);
        //scanner(pos);
    }
}
