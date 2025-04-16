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
    [NodeContent("Float Clamp", "Tasks/Math/Float Clamp", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class FloatClampTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private FloatKey key;

        [SerializeField]
        private FloatKey minValue;

        [SerializeField]
        private FloatKey maxValue;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key == null || minValue == null || maxValue == null)
            {
                return State.Failure;
            }

            key.SetValue(Mathf.Clamp(key.GetValue(), minValue.GetValue(), maxValue.GetValue()));

            return State.Success;
        }
    }
}