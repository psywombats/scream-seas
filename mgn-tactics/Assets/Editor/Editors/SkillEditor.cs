using UnityEditor;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor {

    private PolymorphicFieldUtility targeterUtil;
    private PolymorphicFieldUtility effectorUtil;

    public void OnEnable() {
        targeterUtil = new PolymorphicFieldUtility(typeof(Targeter),
            "Assets/Resources/Database/Targeters/" + ((Skill)target).name + "_targeter.asset");
        effectorUtil = new PolymorphicFieldUtility(typeof(Effector),
            "Assets/Resources/Database/Effectors/" + ((Skill)target).name + "_effector.asset");
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Skill skill = (Skill)target;

        if (!serializedObject.FindProperty("baseTargeter").hasMultipleDifferentValues) {
            skill.baseTargeter = targeterUtil.DrawSelector(skill.baseTargeter);
        }

        if (!serializedObject.FindProperty("baseTargeter").hasMultipleDifferentValues) {
            skill.baseEffect = effectorUtil.DrawSelector(skill.baseEffect);
        }
    }
}
