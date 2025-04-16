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
    [NodeContent("Random Bool", "Tasks/Math/Random Bool", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class RandomBoolTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private BoolKey storedResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (storedResult == null)
            {
                return State.Failure;
            }

            storedResult.SetValue(Random.value > 0.5f);

            return State.Success;
        }
    }
}