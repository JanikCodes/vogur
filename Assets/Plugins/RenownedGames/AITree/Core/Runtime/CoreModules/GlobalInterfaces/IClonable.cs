/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    [System.Obsolete("Use specific IClonable[Type] interfaces.")]
    public interface IClonable<T>
    {
        /// <summary>
        /// Returns a clone of a specific T object.
        /// </summary>
        /// <returns>Type of object to clone.</returns>
        T Clone();
    }
}