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
    [NodeContent("Random Int", "Tasks/Math/Random Int", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class RandomIntTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private IntKey min;

        [SerializeField]
        private IntKey max;

        [SerializeField]
        [NonLocal]
        private IntKey storeResult;

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

            int result = Random.Range(min.GetValue(), max.GetValue());
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}