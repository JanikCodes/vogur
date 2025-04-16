/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    // Temporary solution, after the update, need to move this field in WrapNode.
    internal interface IProcessableNode
    {
        bool IsProcessing();
    }
}