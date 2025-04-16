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
    [NodeContent("Get Euler Angles", "Tasks/Quaternion/Get Euler Angles", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotGetEulerAnglesTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private QuaternionKey quaternion;

        [SerializeField]
        [NonLocal]
        private Vector3Key storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (quaternion == null || storeResult == null)
            {
                return State.Failure;
            }

            storeResult.SetValue(quaternion.GetValue().eulerAngles);
            return State.Success;
        }
    }
}