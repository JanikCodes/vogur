/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    public interface ICloneableNode
    {
        /// <summary>
        /// Clone node with all its stuff.
        /// </summary>
        /// <returns>New cloned copy of node.</returns>
        Node Clone();
    }
}