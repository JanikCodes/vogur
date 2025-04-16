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
    [NodeContent("Int Operator", "Tasks/Math/Int Operator", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class IntOperatorTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private IntKey key1;

        [SerializeField]
        private IntKey key2;

        [SerializeField]
        [NonLocal]
        private IntKey storeResult;

        [Title("Node")]
        [SerializeReference]
        [DropdownReference(PopupStyle = true)]
        private IntOperation operation;

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

            int value1 = key1.GetValue();
            int value2 = key2.GetValue();
            int result = operation.Result(value1, value2);
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}