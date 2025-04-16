/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;

namespace RenownedGames.AITreeEditor
{
    static class DocsMenuItem
    {
        [MenuItem("Tools/AI Tree/Documentation", false, 1)]
        static void Open()
        {
            Help.BrowseURL("https://renownedgames.gitbook.io/ai-tree/");
        }
    }
}