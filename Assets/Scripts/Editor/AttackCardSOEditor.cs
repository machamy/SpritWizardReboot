using DataBase.DataClasses;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MagicCardData))]
public class AttackCardSOEditor : PropertyDrawer{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedObject serializedObject = property.serializedObject;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackCnt"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("image"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillCaster"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRange"));

        AttackType attackType = (AttackType)serializedObject.FindProperty("attackType").enumValueIndex;
        if (attackType == AttackType.beam)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSpread"));
        if (attackType == AttackType.explosion)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spreadRange"));
        if (attackType == AttackType.projectile || attackType == AttackType.projectile){
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pierce"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("projectilePrefab"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("move"));
        // TODO -> specialEffectId 추가

        serializedObject.ApplyModifiedProperties();
    }
}
