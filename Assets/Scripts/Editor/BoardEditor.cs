using System;
using UnityEditor;

namespace Editor
{
    
    [CustomEditor(typeof(Board))]
    public class BoardEditor : UnityEditor.Editor
    {
        
        private SerializedProperty gridSize;
        
        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
        }
    }
}