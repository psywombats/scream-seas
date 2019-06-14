using System.Collections;
using UnityEngine;

/**
 * The effect of a skill/ability/spell etc. Doesn't handle the targeting, which is done by the
 * targeter, the other part of a skill. Abstract to represent melee, heals, teleporting, everything
 * under the sun. The range of the teleport, element of the damage, etc, are the serialized props.
 * Individual instaces of the effector are generated once per useage of the underlying skill.
 */
public abstract class Effector : ActorScriptableObject {

    // === TARGETER HOOKUPS ========================================================================
    // subclasses override as they support

    public virtual IEnumerator ExecuteSingleCellRoutine(SkillResult result, Skill skill, Vector2Int location) {
        Debug.LogError(GetType() + " does not support single cell targeters");
        result.Cancel();
        yield return null;
    }
}
