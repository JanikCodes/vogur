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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OnGUIChangedAttribute : ApexAttribute
    {
        public readonly string name;

        /// <param name="method">
        /// <br>Method format: <b>void OnChanged()</b></br>
        /// </param>
        public OnGUIChangedAttribute(string method)
        {
            this.name = method;
        }
    }
}