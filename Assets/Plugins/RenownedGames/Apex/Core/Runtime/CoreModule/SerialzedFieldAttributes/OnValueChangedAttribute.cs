/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class OnValueChangedAttribute : ApexAttribute
    {
        public readonly string name;

        /// <summary>
        /// <br>Method format: <b>void OnValueChanged()</b></br>
        /// </summary>
        /// <param name="name">Method name.</param>
        public OnValueChangedAttribute(string name)
        {
            this.name = name;
        }
    }
}