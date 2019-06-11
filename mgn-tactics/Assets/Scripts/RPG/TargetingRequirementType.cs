using UnityEngine;
using System.Collections;

public enum TargetingRequirementType {
    NoRequirement,
    RequireEnemy,
    RequireAlly,
    RequireUnitPreferEnemy,
    RequireUnitPreferAlly,
    RequireEmpty,
}
