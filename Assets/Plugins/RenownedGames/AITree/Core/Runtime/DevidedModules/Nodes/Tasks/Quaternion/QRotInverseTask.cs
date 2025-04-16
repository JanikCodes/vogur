/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Quaternion Inverse", "Tasks/Quaternion/Inverse", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotInverseTask : TaskNode
    {
        [SerializeField]
        private QuaternionKey quaternion;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (quaternion == null)
            {
                return State.Failure;
            }

            quaternion.SetValue(Quaternion.Inverse(quaternion.GetValue()));
            return State.Success;
        }
    }
}