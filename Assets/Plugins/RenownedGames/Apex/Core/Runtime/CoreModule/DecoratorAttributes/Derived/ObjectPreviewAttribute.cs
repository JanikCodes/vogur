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
    public sealed class ObjectPreviewAttribute : DecoratorAttribute
    {
        public ObjectPreviewAttribute()
        {
            Height = 150.0f;
        }

        #region [Parameters]
        /// <summary>
        /// Height of the preview window.
        /// </summary>
        public float Height { get; set; }
        #endregion

    }
}