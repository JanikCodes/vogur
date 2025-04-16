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
    public interface IAIPerception
    {
        /// <summary>
        /// Called when source detected.
        /// </summary>
        public event Action<AIPerceptionSource> OnSourceDetect;

        /// <summary>
        /// Called when source losses.
        /// </summary>
        public event Action<AIPerceptionSource> OnSourceLoss;
    }
}