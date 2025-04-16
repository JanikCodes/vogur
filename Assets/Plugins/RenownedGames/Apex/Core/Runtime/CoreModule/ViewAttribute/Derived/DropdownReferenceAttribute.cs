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
    public sealed class DropdownReferenceAttribute : ViewAttribute
    {
        #region [Optional]
        /// <summary>
        /// Use default Unity popup style of dropdown controls.
        /// </summary>
        public bool PopupStyle { get; set; }
        #endregion
    }
}