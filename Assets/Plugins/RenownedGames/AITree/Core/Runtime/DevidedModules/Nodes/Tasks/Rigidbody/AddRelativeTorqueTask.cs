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
    [NodeContent("Add Relative Torque", "Tasks/Rigidbody/Add Relative Torque", IconPath = "Images/Icons/Node/TorqueIcon.png")]
    public class AddRelativeTorqueTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private Vector3Key torque;

        [SerializeField]
        private ForceMode mode = ForceMode.Force;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (torque == null)
            {
                return State.Failure;
            }

            if (target != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody rb = transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddRelativeTorque(torque.GetValue(), mode);
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
