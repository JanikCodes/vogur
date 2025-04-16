/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SuffixAttribute : InlineDecoratorAttribute
    {
        public readonly string label;

        public SuffixAttribute(string label)
        {
            this.label = label;
            Style = "Label";
            Alignment = TextAnchor.MiddleCenter;
            Mute = false;
        }

        #region [Optional Parameters]
        /// <summary>
        /// Name of style of label.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Label alignment of style.
        /// </summary>
        public TextAnchor Alignment { get; set; }

        /// <summary>
        /// Mute label.
        /// </summary>
        public bool Mute { get; set; }
        #endregion
    }
}