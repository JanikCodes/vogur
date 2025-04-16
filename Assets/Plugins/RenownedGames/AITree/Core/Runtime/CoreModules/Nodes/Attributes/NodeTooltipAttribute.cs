/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NodeTooltipAttribute : Attribute
    {
        public readonly string text;

        /// <summary>
        /// Text of node tooltip.
        /// </summary>
        public NodeTooltipAttribute(string text)
        {
            this.text = text;
        }
    }
}