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
    public interface IDecoratorGUI
    {
        /// <summary>
        /// Called for rendering and handling GUI events.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the decorator GUI.</param>
        void OnGUI(Rect position);
    }
}