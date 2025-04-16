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
    [NodeContent("Move Rotation", "Tasks/Rigidbody/Move Rotation", IconPath = "Images/Icons/Node/MoveObjectIcon.png")]
    public class MoveRotationTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private QuaternionKey rotation;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (target != null && rotation != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody rb = transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.MoveRotation(rotation.GetValue());
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
