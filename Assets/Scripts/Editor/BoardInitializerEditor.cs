using System;
using Game.World;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    
    [CustomEditor(typeof(BoardInitializer))]
    public class BoardInitializerEditor : UnityEditor.Editor
    {
        
        private SerializedProperty gridSize;
        
        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Initialize Board"))
            {
                BoardInitializer boardInitializer = (BoardInitializer) target;
                boardInitializer.InitBoard();
            }
        }
    }
}