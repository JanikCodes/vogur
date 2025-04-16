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
    [NodeContent("Capsule Cast 2D", "Tasks/Physics 2D/Capsule Cast 2D", IconPath = "Images/Icons/Node/RayIcon.png")]
    public class CapsuleCast2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector2))]
        private Key origin;

        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector2))]
        private Key direction;

        [SerializeField]
        private Vector2 size = Vector2.one;

        [SerializeField]
        private CapsuleDirection2D capsuleDirection2D;

        [SerializeField]
        private float maxDistance = float.PositiveInfinity;

        [SerializeField]
        private LayerMask cullingLayer;

        [Title("Message")]
        [SerializeField]
        private string message;

        [SerializeField]
        private Key arg;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (origin == null || direction == null)
            {
                return State.Failure;
            }

            Ray2D ray = CalculateRay();
            RaycastHit2D hitInfo = Physics2D.CapsuleCast(ray.origin, size, capsuleDirection2D, 0, ray.direction, maxDistance);
            if (hitInfo.transform != null)
            {
                arg.TryCastValueTo<object>(out object value);
                hitInfo.transform.SendMessage(message, value);
                return State.Success;
            }

            return State.Failure;
        }

        private Ray2D CalculateRay()
        {
            Ray2D ray = new Ray2D();

            if (origin.TryGetPosition2D(out Vector2 originValue))
            {
                ray.origin = originValue;
            }

            if (direction.TryGetPosition2D(out Vector2 directionValue))
            {
                ray.direction = (directionValue - originValue).normalized;
            }

            return ray;
        }
    }
}
