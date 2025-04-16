/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    public interface IKeyLocal
    {
        /// <summary>
        /// Check that key is created as local instance.
        /// </summary>
        /// <returns>True if key is created as local instance. Otherwise false.</returns>
        bool IsLocal();
    }
}