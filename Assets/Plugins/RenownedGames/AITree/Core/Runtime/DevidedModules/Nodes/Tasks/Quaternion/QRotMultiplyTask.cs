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
    [NodeContent("Quaternion Multiply", "Tasks/Quaternion/Multiply", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotMultiplyTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private QuaternionKey quaternionA;

        [SerializeField]
        private QuaternionKey quaternionB;

        [SerializeField]
        [NonLocal]
        private QuaternionKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (quaternionA == null || quaternionB == null || storeResult == null)
            {
                return State.Failure;
            }

            Quaternion result = quaternionA.GetValue() * quaternionB.GetValue();
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}