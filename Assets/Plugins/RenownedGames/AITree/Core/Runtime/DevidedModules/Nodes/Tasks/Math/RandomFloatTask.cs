/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Random Float", "Tasks/Math/Random Float", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class RandomFloatTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private FloatKey min;

        [SerializeField]
        private FloatKey max;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (min == null || max == null || storeResult == null)
            {
                return State.Failure;
            }

            float result = Random.Range(min.GetValue(), max.GetValue());
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}