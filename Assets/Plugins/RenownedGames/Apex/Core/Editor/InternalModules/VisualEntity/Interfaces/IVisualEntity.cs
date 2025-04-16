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
    public interface IVisualEntity
    {
        /// <summary>
        /// Called for rendering and handling visual entity.
        /// </summary>
        /// <param name="position">Rectangle position.</param>
        void OnGUI(Rect position);
    }
}