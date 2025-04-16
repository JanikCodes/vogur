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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Obsolete("Use the [ButtonStyle] attribute instead. [GUIStyle] will be removed in future versions.")]
    public sealed class GUIStyleAttribute : ApexAttribute
    {
        public readonly string name;

        public GUIStyleAttribute(string member)
        {
            this.name = member;
        }
    }
}