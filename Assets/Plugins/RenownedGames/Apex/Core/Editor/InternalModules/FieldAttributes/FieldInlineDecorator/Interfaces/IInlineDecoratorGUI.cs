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
    public interface IInlineDecoratorGUI
    {
        /// <summary>
        /// Called for rendering and handling inline decorator GUI.
        /// </summary>
        /// <param name="position">Calculated position for drawing inline decorator.</param>
        void OnGUI(Rect position);
    }
}