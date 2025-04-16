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
    [NodeContent("Bool Operator", "Tasks/Math/Bool Operator", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class BoolOperatorTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private BoolKey bool1;

        [SerializeField]
        private BoolKey bool2;

        [SerializeField]
        [NonLocal]
        private BoolKey storeResult;

        [Title("Node")]
        [SerializeReference]
        [DropdownReference(PopupStyle = true)]
        private LogicOperation operation;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (bool1 == null || bool2 == null || storeResult == null)
            {
                return State.Failure;
            }

            bool value1 = bool1.GetValue();
            bool value2 = bool2.GetValue();
            bool result = operation.Result(value1, value2);
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}