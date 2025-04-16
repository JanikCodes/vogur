/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace RenownedGames.AITree
{
    public class Note : ScriptableObject
    {
        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public Vector2 size;

        [HideInInspector]
        public string title;

        [HideInInspector]
        public string contents;

#if UNITY_EDITOR
        [HideInInspector]
        public StickyNoteTheme theme;

        [HideInInspector]
        public StickyNoteFontSize fontSize;
#endif
    }
}