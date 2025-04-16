/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.Apex
{
    public sealed class IndentAttribute : ManipulatorAttribute
    {
        public readonly int level;

        public IndentAttribute()
        {
            level = 1;
            Additive = true;
        }

        public IndentAttribute(int level) : this()
        {
            this.level = level;
        }

        #region [Optional]
        public bool Additive { get; set; }
        #endregion
    }
}