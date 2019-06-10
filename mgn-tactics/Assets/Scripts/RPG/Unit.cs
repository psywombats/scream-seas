using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a unit outside of battle. All stuff that persists between fights lives here.
 */
[CreateAssetMenu(fileName = "Unit", menuName = "Data/RPG/Unit")]
public class Unit : ScriptableObject {

    public string unitName;
    public Texture2D appearance;
    public bool unique = true;
    public Alignment align;

    public Skill walkSkill;
    public List<Skill> knownSkills;

    // tempish
    public Item equippedItem;

    // last for shitty reasons
    public StatSet stats;

    // === GETTERS =================================================================================

    public bool IsDead() {
        return stats.Get(StatTag.HP) <= 0;
    }

    public int GetMaxAscent() {
        return (int)stats.Get(StatTag.JUMP);
    }

    public int GetMaxDescent() {
        return (int)stats.Get(StatTag.JUMP) + 1;
    }
}
