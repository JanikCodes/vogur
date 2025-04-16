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
    [NodeContent("Quaternion From To Rotation", "Tasks/Quaternion/From To Rotation", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotFromToRotation : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key fromDirection;

        [SerializeField]
        private Vector3Key toDirection;

        [SerializeField]
        private QuaternionKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (fromDirection == null || toDirection == null || storeResult == null)
            {
                return State.Failure;
            }

            Quaternion quaternion = Quaternion.FromToRotation(fromDirection.GetValue(), toDirection.GetValue());
            storeResult.SetValue(quaternion);

            return State.Success;
        }
    }
}