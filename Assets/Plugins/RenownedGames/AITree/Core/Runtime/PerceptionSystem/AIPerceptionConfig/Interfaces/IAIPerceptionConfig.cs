/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree.PerceptionSystem
{
    public interface IAIPerceptionConfig
    {
        /// <summary>
        /// Called when perception config has updated the target.
        /// <br><b>Param type of(AIPerceptionSource)</b>: New reference of AI perception source.</br>
        /// </summary>
        event Action<AIPerceptionSource> OnTargetUpdated;
    }
}