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
    [NodeContent("Int Clamp", "Tasks/Math/Int Clamp", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class IntClampTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private IntKey key;

        [SerializeField]
        private IntKey minValue;

        [SerializeField]
        private IntKey maxValue;

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