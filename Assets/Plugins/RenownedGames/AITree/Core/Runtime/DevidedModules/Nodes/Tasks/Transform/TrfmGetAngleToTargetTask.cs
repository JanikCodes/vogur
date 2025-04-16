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
    [NodeContent("Get Angle To Target", "Tasks/Transform/Get Angle To Target", IconPath = "Images/Icons/Node/TransformIcon.png")]
    public class TrfmGetAngleToTargetTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [SerializeField]
        [NonLocal]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key target;

        [SerializeField]
        [NonLocal]
        private FloatKey storeAngle;

        [Title("Node")]
        [SerializeField]
        private bool ignoreHeight;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null || target == null || storeAngle == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            if(!target.TryGetPosition(out Vector3 targetPosition))
            {
                return State.Failure;
            }

            Vector3 forward = _transform.forward;
            Vector3 direction = targetPosition - _transform.position;
            if (ignoreHeight)
            {
                forward.y = 0f;
                forward.Normalize();

                direction.y = 0f;
            }
            direction.Normalize();

            float angle = Vector3.Angle(forward, direction);
            storeAngle.SetValue(angle);

            return State.Success;
        }
    }
}