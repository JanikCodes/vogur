/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.ApexEditor
{
    public interface IInlineDecoratorInitialize
    {
        /// <summary>
        /// Called once when initializing element inline decorator.
        /// </summary>
        /// <param name="element">Serialized element with InlineDecoratorAttribute.</param>
        /// <param name="inlineDecoratorAttribute">InlineDecoratorAttribute of serialized element.</param>
        /// <param name="label">Label of serialized property.</param>
        void Initialize(SerializedField element, InlineDecoratorAttribute inlineDecoratorAttribute, GUIContent label);
    }
}