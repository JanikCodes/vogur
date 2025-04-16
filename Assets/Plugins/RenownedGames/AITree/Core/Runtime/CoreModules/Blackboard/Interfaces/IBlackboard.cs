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
    public interface IBlackboard
    {
        /// <summary>
        /// Add new key to blackboard.
        /// </summary>
        /// <param name="key">Key reference.</param>
        void AddKey(Key key);

        /// <summary>
        /// Remove key from blackboard.
        /// </summary>
        /// <param name="key">Key reference.</param>
        void DeleteKey(Key key);
    }
}