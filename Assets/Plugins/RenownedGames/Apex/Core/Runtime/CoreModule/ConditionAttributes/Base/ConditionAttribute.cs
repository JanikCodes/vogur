/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ConditionAttribute : ManipulatorAttribute
    {
        public readonly string name;
        public readonly object arg;

        public ConditionAttribute(string name)
        {
            this.name = name;
        }

        public ConditionAttribute(string name, object arg)
        {
            this.name = name;
            this.arg = arg;
        }
    }
}