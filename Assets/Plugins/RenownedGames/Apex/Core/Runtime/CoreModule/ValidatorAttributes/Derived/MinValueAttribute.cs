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
    public class MinValueAttribute : ValidatorAttribute
    {
        public readonly float value;
        public readonly string property;

        public MinValueAttribute(float value)
        {
            this.value = value;
        }

        public MinValueAttribute(string property)
        {
            this.property = property;
        }
    }
}