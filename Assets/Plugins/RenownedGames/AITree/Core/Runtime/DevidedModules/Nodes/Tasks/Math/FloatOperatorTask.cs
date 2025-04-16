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
    [NodeContent("Float Operator", "Tasks/Math/Float Operator", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class FloatOperatorTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private FloatKey key1;

        [SerializeField]
        private FloatKey key2;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

        [Title("Node")]
        [SerializeReference]
        [DropdownReference(PopupStyle = true)]
        private FloatOperation operation;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key1 == null || key2 == null || storeResult == null)
            {
                return State.Failure;
            }

            float value1 = key1.GetValue();
            float value2 = key2.GetValue();
            float result = operation.Result(value1, value2);
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}