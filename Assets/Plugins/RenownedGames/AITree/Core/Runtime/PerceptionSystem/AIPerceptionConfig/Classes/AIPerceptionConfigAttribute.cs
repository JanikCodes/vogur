/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree.PerceptionSystem
{
    [Obsolete("Use RenownedGames.Apex.SearchContent instead.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AIPerceptionConfigAttribute : Attribute
    {
        public readonly string path;

        public AIPerceptionConfigAttribute(string path)
        {
            this.path = path;
            Icon = string.Empty;
        }

        #region [Optional]
        public string Icon { get; set; }
        #endregion
    }
}
