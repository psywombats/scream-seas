using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomPropertyDrawer(typeof(StatSet))]
public class StatSetDrawer : PropertyDrawer {

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
        SerializedProperty serialDictionary = property.FindPropertyRelative("serializedStats");
        SerializedProperty keys = serialDictionary.FindPropertyRelative("keys");
        SerializedProperty values = serialDictionary.FindPropertyRelative("values");

        EditorGUI.BeginProperty(pos, label, property);

        GUIStyle fieldStyle = new GUIStyle(GUI.skin.textArea) {
            fixedWidth = 80
        };

        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        var allValues = Enum.GetValues(typeof(StatTag));
        int seen = 0;
        for (int i = 0; i < allValues.Length; i += 1) {
            Stat stat = Stat.Get((int)allValues.GetValue(i));
            if (stat == null) continue;
            EditorGUI.LabelField(
                new Rect(18 + pos.x, 24 + pos.y + seen * 18, pos.width, EditorGUIUtility.singleLineHeight),
                stat.nameShort + ": ");
            if (stat.useBinaryEditor) {
                SetStatValue(property, stat, EditorGUI.Toggle(
                    new Rect(pos.x + 100, 24 + pos.y + seen * 18, pos.width, EditorGUIUtility.singleLineHeight),
                    (GetStatValue(property, stat) > 0.0f)) ? 1.0f : 0.0f);
            } else {
                SetStatValue(property, stat, EditorGUI.FloatField(
                    new Rect(pos.x + 100, 24 + pos.y + seen * 18, pos.width, EditorGUIUtility.singleLineHeight),
                    GetStatValue(property, stat),
                    fieldStyle));
            }
            seen += 1;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * (Enum.GetValues(typeof(StatTag)).Length + 1);
    }

    private float GetStatValue(SerializedProperty property, Stat stat) {
        SerializedProperty serialDictionary = property.FindPropertyRelative("serializedStats");
        SerializedProperty keys = serialDictionary.FindPropertyRelative("keys");
        SerializedProperty values = serialDictionary.FindPropertyRelative("values");

        for (int i = 0; i < keys.arraySize; i += 1) {
            if (keys.GetArrayElementAtIndex(i).stringValue == stat.nameShort) {
                return values.GetArrayElementAtIndex(i).floatValue;
            }
        }
        return stat.combinator.Identity();
    }

    private void SetStatValue(SerializedProperty property, Stat stat, float value) {
        SerializedProperty serialDictionary = property.FindPropertyRelative("serializedStats");
        SerializedProperty keys = serialDictionary.FindPropertyRelative("keys");
        SerializedProperty values = serialDictionary.FindPropertyRelative("values");

        for (int i = 0; i < keys.arraySize; i += 1) {
            if (keys.GetArrayElementAtIndex(i).stringValue == stat.nameShort) {
                values.GetArrayElementAtIndex(i).floatValue = value;
                return;
            }
        }

        keys.arraySize += 1;
        values.arraySize += 1;
        keys.InsertArrayElementAtIndex(keys.arraySize - 1);
        values.InsertArrayElementAtIndex(values.arraySize - 1);
        keys.GetArrayElementAtIndex(keys.arraySize - 1).stringValue = stat.nameShort;
        values.GetArrayElementAtIndex(values.arraySize - 1).floatValue = value;
    }
}