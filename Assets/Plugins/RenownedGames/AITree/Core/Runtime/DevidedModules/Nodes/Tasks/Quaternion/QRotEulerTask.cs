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
    [NodeContent("Quaternion Euler", "Tasks/Quaternion/Euler", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotEulerTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key eulerAngles;

        [SerializeField]
        private QuaternionKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (eulerAngles == null || storeResult == null)
            {
                return State.Failure;
            }

            Quaternion result = Quaternion.Euler(eulerAngles.GetValue());
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}