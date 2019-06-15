using System.Collections;
using UnityEngine;

/**
 * ...You walk to the selected location.
 */
public class WalkEffect : Effector {

    public int additionalEnergyPerTile = 10;

    public override IEnumerator ExecuteSingleCellRoutine(SkillResult result, Vector2Int location) {
        Vector2Int originalPos = actor.position;
        yield return mapEvent.PathToRoutine(location);
        skill.FinalizeSkillResult(result);
        result.value.timeExpended += additionalEnergyPerTile * Map.ManhattanDistance(originalPos, actor.position);
    }
}
