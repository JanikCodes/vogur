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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        public readonly float value;
        public readonly string property;

        public MaxValueAttribute(float value)
        {
            this.value = value;
        }

        public MaxValueAttribute(string property)
        {
            this.property = property;
            this.value = 100;
        }
    }
}