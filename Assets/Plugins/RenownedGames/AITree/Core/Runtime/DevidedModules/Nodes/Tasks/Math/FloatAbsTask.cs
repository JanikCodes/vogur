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
    [NodeContent("Float Abs", "Tasks/Math/Float Abs", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class FloatAbsTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private FloatKey key;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key == null)
            {
                return State.Failure;
            }

            key.SetValue(Mathf.Abs(key.GetValue()));

            return State.Success;
        }
    }
}