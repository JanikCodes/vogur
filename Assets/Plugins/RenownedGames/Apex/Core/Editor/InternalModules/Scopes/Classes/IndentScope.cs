/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.ApexEditor
{
    [System.Obsolete]
    public sealed class IndentScope : GUI.Scope
    {
        private readonly int lastLevel;

        public IndentScope(int level) { }

        public IndentScope() : this(1) { }

        protected override void CloseScope() { }
    }
}