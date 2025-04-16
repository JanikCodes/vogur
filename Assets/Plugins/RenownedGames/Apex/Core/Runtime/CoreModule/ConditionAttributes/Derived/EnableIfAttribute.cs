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
    public sealed class EnableIfAttribute : ConditionAttribute
    {
        public EnableIfAttribute(string member) : base(member) { }

        public EnableIfAttribute(string member, object comparer) : base(member, comparer) { }
    }
}