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
    public interface IKeySynced
    {
        /// <summary>
        /// This is used to determine if the Key will be synchronized across all instances of the Blackboard.
        /// </summary>
        bool IsSync();
    }
}