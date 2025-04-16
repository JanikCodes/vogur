/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ExLibEditor;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    public sealed class AITreeIntegrationWindow : IntegrationWindow
    {
        protected override string GetTitle()
        {
            return "AI Tree Integrations";
        }

        protected override string GetExactDirectory()
        {
            return "RenownedGames/AITree";
        }

        [MenuItem("Tools/AI Tree/Integrations",false, 51)]
        public static void Open()
        {
            Open<AITreeIntegrationWindow>(new GUIContent("Integrations"));
        }
    }
}