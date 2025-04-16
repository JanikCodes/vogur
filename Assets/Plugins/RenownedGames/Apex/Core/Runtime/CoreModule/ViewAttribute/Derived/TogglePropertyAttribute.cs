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
    public sealed class TogglePropertyAttribute : ViewAttribute
    {
        public readonly string boolValue;

        public TogglePropertyAttribute(string boolValue)
        {
            this.boolValue = boolValue;
        }

        #region [Optional Parameters]
        /// <summary>
        /// Hide property, otherwise property will be disabled.
        /// </summary>
        public bool Hide { get; set; }
        #endregion
    }
}