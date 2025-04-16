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
    public sealed class ProgressBarAttribute : ViewAttribute
    {
        public readonly string text;

        public ProgressBarAttribute()
        {
            text = string.Empty;
            Height = 20;
        }

        public ProgressBarAttribute(string text) : this()
        {
            this.text = text;
        }

        #region [Optional Properties]
        public float Height { get; set; }
        #endregion
    }
}