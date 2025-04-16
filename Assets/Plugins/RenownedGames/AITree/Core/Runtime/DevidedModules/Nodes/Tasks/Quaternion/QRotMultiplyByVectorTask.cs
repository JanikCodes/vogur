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
    [NodeContent("Quaternion Multiply By Vector", "Tasks/Quaternion/Multiply By Vector", IconPath = "Images/Icons/Node/QuaternionIcon.png")]
    public class QRotMultiplyByVectorTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private QuaternionKey quaternion;

        [SerializeField]
        private Vector3Key vector3;

        [SerializeField]
        [NonLocal]
        private Vector3Key storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (quaternion == null || vector3 == null || storeResult == null)
            {
                return State.Failure;
            }

            Vector3 result = quaternion.GetValue() * vector3.GetValue();
            storeResult.SetValue(result);

            return State.Success;
        }
    }
}