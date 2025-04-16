/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.ApexEditor
{
    public interface IGUIChangedCallback
    {
        /// <summary>
        /// Called when GUI has been changed.
        /// </summary>
        event Action GUIChanged;
    }
}
