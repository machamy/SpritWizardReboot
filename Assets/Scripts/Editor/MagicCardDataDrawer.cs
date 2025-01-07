//
// using UnityEditor;
// using UnityEngine;
//
//
//     [CustomPropertyDrawer(typeof(MagicCardData))]
//     public class MagicCardDataDrawer : PropertyDrawer
//     {
//         
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);
//         
//             position = EditorGUI.IndentedRect(position);
//             var propHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//         
//             var cardName = property.FindPropertyRelative("cardName");
//             var description = property.FindPropertyRelative("description");
//             var rarity = property.FindPropertyRelative("rarity");
//             var damage = property.FindPropertyRelative("attackDamage");
//             var attackCnt = property.FindPropertyRelative("attackCount");
//             var cost = property.FindPropertyRelative("cost");
//             var frontImage = property.FindPropertyRelative("frontImage");
//             var backImage = property.FindPropertyRelative("backImage");
//             var skillCaster = property.FindPropertyRelative("skillCaster");
//             var attackType = property.FindPropertyRelative("attackType");
//             var attackHeight = property.FindPropertyRelative("attackHeight");
//             var attackWidth = property.FindPropertyRelative("attackWidth");
//             var attackSpread = property.FindPropertyRelative("attackSpread");
//             var spreadRange = property.FindPropertyRelative("spreadRange");
//             var pierce = property.FindPropertyRelative("pierce");
//             var projectilePrefabName = property.FindPropertyRelative("projectilePrefabName");
//             var move = property.FindPropertyRelative("move");
//         
//             position.height = propHeight;
//             EditorGUI.PropertyField(position, cardName);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, description);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, rarity);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, damage);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, attackCnt);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, cost);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, frontImage);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, backImage);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, skillCaster);
//             position.y += propHeight;
//         
//             EditorGUI.PropertyField(position, attackType);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, attackHeight);
//             position.y += propHeight;
//             EditorGUI.PropertyField(position, attackWidth);
//             position.y += propHeight;
//             
//
//             AttackType attackTypeEnum = (AttackType)attackType.enumValueIndex;
//
//             if (attackTypeEnum == AttackType.beam)
//             {
//                 EditorGUI.PropertyField(position, attackSpread);
//                 position.y += propHeight;
//             }
//             else if (attackTypeEnum == AttackType.explosion)
//             {
//                 EditorGUI.PropertyField(position, spreadRange);
//                 position.y += propHeight;
//             }
//             else if (attackTypeEnum == AttackType.projectile)
//             {
//                 EditorGUI.PropertyField(position, pierce);
//                 position.y += propHeight;
//                 EditorGUI.PropertyField(position, projectilePrefabName);
//                 position.y += propHeight;
//             }
//         
//             EditorGUI.PropertyField(position, move);
//             position.y += propHeight;
//         
//             // EditorGUI.PropertyField(position, property.FindPropertyRelative("specialEffectId"));
//             // position.y += propHeight;
//             EditorGUI.EndProperty();
//         }
//         
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return 18 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
//         }
//     }
//
