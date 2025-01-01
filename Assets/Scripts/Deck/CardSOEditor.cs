using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardSO))]
public class CardSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CardSO cardSO = (CardSO)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));

        if (cardSO.cardType == CardType.Attack)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveDistance"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cost"));

        if (cardSO.cardType == CardType.Rune)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damageCalculateType"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));

        if (cardSO.cardType == CardType.Rune)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackCntCalculateType"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackCnt"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("image"));

        serializedObject.ApplyModifiedProperties();
    }
}
