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
    public interface IViewInitialization
    {
        /// <summary>
        /// Called once when initializing element view.
        /// </summary>
        /// <param name="element">Serialized element with ViewAttribute.</param>
        /// <param name="viewAttribute">ViewAttribute of serialized element.</param>
        /// <param name="label">Label of serialized element.</param>
        void Initialize(SerializedField element, ViewAttribute viewAttribute, GUIContent label);
    }
}