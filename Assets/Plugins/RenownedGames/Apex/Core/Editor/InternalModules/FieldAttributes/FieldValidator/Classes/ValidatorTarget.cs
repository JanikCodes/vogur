/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.ApexEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ValidatorTarget : Attribute
    {
        public readonly Type target;

        public ValidatorTarget(Type target)
        {
            this.target = target;
        }
    }
}