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
    public interface INode
    {
        /// <summary>
        /// Updating state of the node in behaviour tree execution order.
        /// </summary>
        /// <returns>State of node after updating.</returns>
        State Update();
    }
}