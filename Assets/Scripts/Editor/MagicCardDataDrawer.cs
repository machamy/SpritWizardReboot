using DataBase.DataClasses;
using UnityEditor;
using UnityEngine;


    [CustomPropertyDrawer(typeof(MagicCardData))]
    public class MagicCardDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            position = EditorGUI.IndentedRect(position);
            var propHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        
            var cardName = property.FindPropertyRelative("cardName");
            var description = property.FindPropertyRelative("description");
            var rarity = property.FindPropertyRelative("rarity");
            var damage = property.FindPropertyRelative("damage");
            var attackCnt = property.FindPropertyRelative("attackCnt");
            var cost = property.FindPropertyRelative("cost");
            var image = property.FindPropertyRelative("image");
            var skillCaster = property.FindPropertyRelative("skillCaster");
            var attackType = property.FindPropertyRelative("attackType");
            var attackRange = property.FindPropertyRelative("attackRange");
            var attackSpread = property.FindPropertyRelative("attackSpread");
            var spreadRange = property.FindPropertyRelative("spreadRange");
            var pierce = property.FindPropertyRelative("pierce");
            var projectilePrefab = property.FindPropertyRelative("projectilePrefab");
            var move = property.FindPropertyRelative("move");
        
            position.height = propHeight;
            EditorGUI.PropertyField(position, cardName);
            position.y += propHeight;
            EditorGUI.PropertyField(position, description);
            position.y += propHeight;
            EditorGUI.PropertyField(position, rarity);
            position.y += propHeight;
            EditorGUI.PropertyField(position, damage);
            position.y += propHeight;
            EditorGUI.PropertyField(position, attackCnt);
            position.y += propHeight;
            EditorGUI.PropertyField(position, cost);
            position.y += propHeight;
            EditorGUI.PropertyField(position, image);
            position.y += propHeight;
            EditorGUI.PropertyField(position, skillCaster);
            position.y += propHeight;
        
            EditorGUI.PropertyField(position, attackType);
            position.y += propHeight;
            EditorGUI.PropertyField(position, attackRange);
            position.y += propHeight;

            AttackType attackTypeEnum = (AttackType)attackType.enumValueIndex;

            if (attackTypeEnum == AttackType.beam)
            {
                EditorGUI.PropertyField(position, attackSpread);
                position.y += propHeight;
            }
            else if (attackTypeEnum == AttackType.explosion)
            {
                EditorGUI.PropertyField(position, spreadRange);
                position.y += propHeight;
            }
            else if (attackTypeEnum == AttackType.projectile)
            {
                EditorGUI.PropertyField(position, pierce);
                position.y += propHeight;
                EditorGUI.PropertyField(position, projectilePrefab);
                position.y += propHeight;
            }
        
            EditorGUI.PropertyField(position, move);
            position.y += propHeight;
        
            // EditorGUI.PropertyField(position, property.FindPropertyRelative("specialEffectId"));
            // position.y += propHeight;

            EditorGUI.EndProperty();
        }
    }

