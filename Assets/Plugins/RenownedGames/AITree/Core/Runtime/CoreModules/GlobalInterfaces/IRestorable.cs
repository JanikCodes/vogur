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
    public interface IRestorable
    {
        /// <summary>
        /// Resets parameters to their original values.
        /// </summary>
        void Restore();
    }
}