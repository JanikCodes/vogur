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
    [NodeContent("Inverse Transform Direction", "Tasks/Transform/Inverse Transform Direction", IconPath = "Images/Icons/Node/TransformIcon.png")]
    public class TrfmInverseDirectionTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        private Vector3Key worldDirection;

        [SerializeField]
        [NonLocal]
        private Vector3Key storeResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || worldDirection == null || storeResult == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            storeResult.SetValue(_transform.InverseTransformDirection(worldDirection.GetValue()));
            return State.Success;
        }
    }
}