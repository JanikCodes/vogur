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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class PrefixAttribute : InlineDecoratorAttribute
    {
        public readonly string label;

        public PrefixAttribute(string label)
        {
            this.label = label;
            Style = "Label";
            BeforeField = false;
        }

        #region [Optional Parameters]
        public string Style { get; set; }

        public bool BeforeField { get; set; }
        #endregion
    }
}