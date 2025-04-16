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
    [NodeContent("Add Force At Position", "Tasks/Rigidbody/Add Force At Position", IconPath = "Images/Icons/Node/ForceIcon.png")]
    public class AddForceAtPositionTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private Vector3Key force;

        [SerializeField]
        private Vector3Key position;

        [SerializeField]
        private ForceMode mode = ForceMode.Force;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (force == null || position == null)
            {
                return State.Failure;
            }

            if (target != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody rb = transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForceAtPosition(force.GetValue(), position.GetValue(), mode);
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
