using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AdjacentTileTargeter : Targeter {

    public TargetingRequirementType targetingReq;

    protected override IEnumerator InternalExecuteRoutine(Skill skill, SkillResult result) {
        DirectionCursor cursor = controller.SpawnDirCursor(actor.position);

        List<OrthoDir> dirs = new List<OrthoDir>();
        foreach (OrthoDir dir in Enum.GetValues(typeof(OrthoDir))) {
            Vector2Int pos = actor.position + dir.XY3D();
            if (controller.PositionMeetsTargetingReq(actor, targetingReq, pos)) {
                dirs.Add(dir);
            }
        }

        if (dirs.Count == 0) {
            Debug.Assert(false, "No valid directions");
            result.Cancel();
        }

        Result<OrthoDir> dirResult = new Result<OrthoDir>();
        while (!result.finished) {
            yield return cursor.SelectTargetDirRoutine(dirResult, actor, dirs, controller.GenericScanner(), true);
        }
        cursor.Disable();

        if (dirResult.canceled) {
            result.Cancel();
        } else {
            Vector2Int pos = actor.position + dirResult.value.XY3D();
            yield return skill.effect.ExecuteSingleCellRoutine(result, skill, pos);
        }
    }
}
