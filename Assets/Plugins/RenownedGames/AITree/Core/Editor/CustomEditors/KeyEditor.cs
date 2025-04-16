/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    [CustomEditor(typeof(Key), true)]
    public sealed class KeyEditor : Editor
    {
        private GUIStyle labelStyle;

        public override void OnInspectorGUI()
        {
            if(labelStyle == null)
            {
                labelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                labelStyle.wordWrap = true;
            }

            GUILayout.Space(10);
            GUILayout.Label("Use Blackboard Details to view key info of specific blackboard and Blackboard Viewer to view current value of key at play time.", labelStyle);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Open Blackboard Details", EditorStyles.miniButtonLeft))
                {
                    BlackboardWindow.Open();
                    BlackboardDetailsWindow.Open();
                }

                if (GUILayout.Button("Open Blackboard Viewer", EditorStyles.miniButtonRight))
                {
                    BlackboardViewerWindow.Open();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
