/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    internal static class AIPerceptionSourceUtility 
    {
        public static Dictionary<Collider, AIPerceptionSource> Sources;

        static AIPerceptionSourceUtility()
        {
            Sources = new Dictionary<Collider, AIPerceptionSource>();
        }

        /// <summary>
        /// Check if AI Perception Source attached to this collider.
        /// </summary>
        /// <param name="collider">Gameobject collider.</param>
        /// <returns>AI Perception Source of collider, otherwise null.</returns>
        public static AIPerceptionSource GetAIPerceptionSource(this Collider collider)
        {
            if(Sources.TryGetValue(collider, out AIPerceptionSource source))
            {
                return source;
            }
            return null;
        }
    }
}