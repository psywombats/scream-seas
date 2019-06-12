using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/RPG/Skill")]
public class Skill : ScriptableObject {

    public string skillName;
    public int apCost;

    public Targeter targeter;
    public Effector effect;

    public Skill(Targeter targeter, Effector effect) {
        this.targeter = targeter;
        this.effect = effect;
    }

    public IEnumerator PlaySkillRoutine(BattleUnit actor, Result<Effector> effectResult) {
        Targeter targeter = Instantiate(this.targeter);
        Effector effect = Instantiate(this.effect);
        effect.actor = actor;
        targeter.actor = actor;
        yield return targeter.ExecuteRoutine(effect, effectResult);
    }

    // check if the skill is usable by the actor, given the current battle conditions
    // this doesn't imply there are valid targets, just that its targeter can be run to begin with
    public bool IsUsable(BattleUnit actor) {
        return true;
    }
}
