using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/RPG/Skill")]
public class Skill : ScriptableObject {

    public string skillName;
    public int energyCost;

    public Targeter baseTargeter;
    public Effector baseEffect;
    public Targeter currentTargeter { get; private set; }
    public Effector currentEffect { get; private set; }

    public IEnumerator PlaySkillRoutine(BattleUnit actor, SkillResult result) {
        currentTargeter = Instantiate(baseTargeter);
        currentTargeter.actor = actor;
        currentTargeter.skill = this;
        currentEffect = Instantiate(baseEffect);
        currentEffect.actor = actor;
        currentEffect.skill = this;
        yield return currentTargeter.ExecuteRoutine(result);
    }

    // check if the skill is usable by the actor, given the current battle conditions
    // this doesn't imply there are valid targets, just that its targeter can be run to begin with
    public bool IsUsable(BattleUnit actor) {
        return true;
    }

    public void FinalizeSkillResult(SkillResult result) {
        result.value = new SkillResult.SkillResultValue();
        result.value.timeExpended = energyCost;
    }
}
