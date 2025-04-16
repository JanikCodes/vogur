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
    public sealed class LabelAttribute : ManipulatorAttribute
    {
        public readonly string name;

        public LabelAttribute(string name)
        {
            this.name = name;
        }
    }
}