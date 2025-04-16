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
    public interface IValidatorInitialization
    {
        /// <summary>
        /// Called once when initializing PropertyValidator.
        /// </summary>
        /// <param name="property">Serialized element with ValidatorAttribute.</param>
        /// <param name="attribute">ValidatorAttribute of serialized element.</param>
        /// <param name="label">Label of serialized property.</param>
        void Initialize(SerializedField element, ValidatorAttribute attribute, GUIContent label);
    }
}