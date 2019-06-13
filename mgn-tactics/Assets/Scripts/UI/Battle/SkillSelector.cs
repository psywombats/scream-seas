using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ListSelector))]
public class SkillSelector : MonoBehaviour {

    private ListSelector selector { get { return GetComponent<ListSelector>(); } }

    public ListStringCell cellPrefab;

    private BattleUnit currentActor;

    public IEnumerator SelectSkillRoutine(Result<Skill> result, BattleUnit actor) {
        currentActor = actor;
        yield return selector.SelectAndPersistRoutine(result, actor.unit.knownSkills, CellConstructor);
    }

    private ListCell CellConstructor(Skill skill) {
        ListStringCell cell = Instantiate(cellPrefab.gameObject).GetComponent<ListStringCell>();
        cell.Populate(skill.skillName);
        cell.SetSelectable(skill.IsUsable(currentActor));
        return cell;
    }
}
