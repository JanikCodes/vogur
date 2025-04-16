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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ButtonHeightAttribute : ApexAttribute 
    {
        public readonly float height;

        public ButtonHeightAttribute(float height)
        {
            this.height = height;
        }
    }
}