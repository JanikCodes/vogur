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
    [NodeContent("Bool Flip", "Tasks/Math/Bool Flip", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class BoolFlipTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private BoolKey boolKey;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (boolKey == null)
            {
                return State.Failure;
            }

            boolKey.SetValue(!boolKey.GetValue());

            return State.Success;
        }
    }
}