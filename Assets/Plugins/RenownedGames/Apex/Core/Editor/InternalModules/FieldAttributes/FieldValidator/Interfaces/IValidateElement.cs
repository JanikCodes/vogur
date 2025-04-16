/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.ApexEditor
{
    public interface IValidateElement
    {
        /// <summary>
        /// Called every inspector update time before drawing property.
        /// </summary>
        /// <param name="element">Serialized element with ValidatorAttribute.</param>
        void Validate(SerializedField element);
    }
}