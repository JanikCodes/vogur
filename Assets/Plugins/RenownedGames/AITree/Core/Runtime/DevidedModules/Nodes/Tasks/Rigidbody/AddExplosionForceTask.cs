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
    [NodeContent("Add Explosion Force", "Tasks/Rigidbody/Add Explosion Force", IconPath = "Images/Icons/Node/ExplosionIcon.png")]
    public class AddExplosionForceTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private TransformKey target;

        [SerializeField]
        private FloatKey explosionForce;

        [SerializeField]
        private Vector3Key explosionPosition;

        [SerializeField]
        private FloatKey explosionRadius;

        [SerializeField]
        private float upwardsModifier = 0f;

        [SerializeField]
        private ForceMode mode = ForceMode.Force;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (explosionForce == null || explosionPosition == null || explosionRadius == null)
            {
                return State.Failure;
            }

            if (target != null && target.TryGetTransform(out Transform transform))
            {
                Rigidbody rb = transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce.GetValue(), explosionPosition.GetValue(), explosionRadius.GetValue(), upwardsModifier, mode);
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}
