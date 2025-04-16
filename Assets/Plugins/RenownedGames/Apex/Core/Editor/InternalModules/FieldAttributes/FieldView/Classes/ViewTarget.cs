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
    public sealed class ViewTarget : Attribute
    {
        public readonly Type target;

        public ViewTarget(Type target)
        {
            this.target = target;
        }
    }
}