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
    public interface IKeyQuery
    {
        /// <summary>
        /// Compare two key by specified condition.
        /// </summary>
        /// <returns>Result of query between two key.</returns>
        bool Result();
    }
}