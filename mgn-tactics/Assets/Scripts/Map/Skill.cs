using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/RPG/Skill")]
public class Skill : ScriptableObject {

    public string skillName;
    public int energyCost;

    public Targeter targeter;
    public Effector effect;

    public Skill(Targeter targeter, Effector effect) {
        this.targeter = targeter;
        this.effect = effect;
    }

    public IEnumerator PlaySkillRoutine(BattleUnit actor, SkillResult result) {
        Targeter targeter = Instantiate(this.targeter);
        Effector effect = Instantiate(this.effect);
        effect.actor = actor;
        targeter.actor = actor;
        yield return targeter.ExecuteRoutine(this, result);
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
