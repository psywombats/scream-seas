using System.Collections;

/**
 * Abstract to cover single-empty-square-at-range vs one-direction. Serialized props on each
 * instance are stuff like range, radius... mostly simple.
 **/
public abstract class Targeter : ActorScriptableObject {

    /**
     * Acquire the targets, pass them to the effector via the appropriate method.
     */
    public IEnumerator ExecuteRoutine(Skill skill, SkillResult result) {
        yield return ExecuteRoutine(skill, result);
    }

    protected abstract IEnumerator InternalExecuteRoutine(Skill skill, SkillResult result);
}
