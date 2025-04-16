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
    [NodeContent("Quaternion Angle Axis", "Tasks/Quaternion/Angle Axis", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotAngleAxisTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private FloatKey angle;

        [SerializeField]
        private Vector3Key axis;

        [SerializeField]
        private QuaternionKey storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (angle == null || axis == null || storeResult == null)
            {
                return State.Failure;
            }

            Quaternion quaternion = Quaternion.AngleAxis(angle.GetValue(), axis.GetValue());
            storeResult.SetValue(quaternion);

            return State.Success;
        }
    }
}