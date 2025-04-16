/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.ExLibEditor.Windows
{
    public interface IExSearchWindowEntry
    {
        /// <summary>
        /// Add new entry.
        /// </summary>
        /// <param name="content">The text and icon of the search entry.</param>
        /// <param name="data">A user specified object for attaching application specific data to a search tree entry.</param>
        /// <param name="onSelect">Action with data argument, which called after entry is selected.</param>
        void AddEntry(GUIContent content, object data, Action<object> onSelect);
    }
}