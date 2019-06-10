using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatSet))]
public class StatSetDrawer : PropertyDrawer {

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
        SerializedProperty serialDictionary = property.FindPropertyRelative("serializedStats");
        SerializedProperty keys = serialDictionary.FindPropertyRelative("keys");
        SerializedProperty values = serialDictionary.FindPropertyRelative("values");

        GUIStyle fieldStyle = new GUIStyle(GUI.skin.textArea);
        fieldStyle.fixedWidth = 80;

        EditorGUILayout.LabelField("Stats");

        for (int i = 0; i < keys.arraySize; i += 1) {
            SerializedProperty serializedValue = values.GetArrayElementAtIndex(i);
            Stat stat = Stat.Get(keys.GetArrayElementAtIndex(i).enumValueIndex);
            float value = serializedValue.floatValue;
            
            if (stat.useBinaryEditor) {
                serializedValue.floatValue = EditorGUILayout.Toggle(stat.nameShort, value > 0) ? 1.0f : 0.0f;
            } else {
                serializedValue.floatValue = EditorGUILayout.FloatField(stat.nameShort, value);
            }

        }
    }
}