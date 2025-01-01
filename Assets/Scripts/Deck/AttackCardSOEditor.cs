using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackCardSO))]
public class AttackCardSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AttackCardSO cardSO = (AttackCardSO)target;

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

        if (cardSO.attackType != AttackType.explosion)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSpread"));
        }
        if (cardSO.attackSpread == AttackSpread.radial)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spreadRange"));
        }
        if (cardSO.attackType == AttackType.projectile)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pierce"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("projectilePrefab"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("move"));
        // TODO -> specialEffectId 추가

        serializedObject.ApplyModifiedProperties();
    }
}
