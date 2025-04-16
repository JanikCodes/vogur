/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;

namespace RenownedGames.AITree
{
    public interface IBlackboardKeys
    {
        /// <summary>
        /// Iterate through all keys in blackboard.
        /// </summary>
        IEnumerable<Key> Keys { get; }
    }
}