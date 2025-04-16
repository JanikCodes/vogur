/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.ApexEditor
{
    public interface IDrawerHeight
    {
        /// <summary>
        /// Get height which needed to element drawer.
        /// </summary>
        /// <param name="element">Serialized element with DrawerAttribute.</param>
        /// <param name="label">Label of serialized element.</param>
        float GetHeight(SerializedField element, GUIContent label);
    }
}