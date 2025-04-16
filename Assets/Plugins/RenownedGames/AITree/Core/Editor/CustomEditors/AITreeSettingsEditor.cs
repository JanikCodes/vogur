/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ApexEditor;
using UnityEditor;

namespace RenownedGames.AITreeEditor
{
    [CustomEditor(typeof(AITreeSettings))]
    sealed class AITreeSettingsEditor : AEditor
    {
        /// <summary>
        /// Implement this function to make a custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GetRootContainer().HasObjectChanged())
            {
                AITreeSettings settings = (AITreeSettings)target;
                settings.Save();
            }
        }
    }
}