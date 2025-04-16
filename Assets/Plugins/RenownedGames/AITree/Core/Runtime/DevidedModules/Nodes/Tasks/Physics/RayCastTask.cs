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
    [NodeContent("Ray Cast", "Tasks/Physics/Ray Cast", IconPath = "Images/Icons/Node/RayIcon.png")]
    public class RayCastTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key origin;

        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key direction;

        [SerializeField]
        private float maxDistance = float.PositiveInfinity;

        [SerializeField]
        private LayerMask cullingLayer;

        [SerializeField]
        private QueryTriggerInteraction queryTriggerInteraction;

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

            Ray ray = CalculateRay();
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, cullingLayer, queryTriggerInteraction))
            {
                arg.TryCastValueTo<object>(out object value);
                hitInfo.transform.SendMessage(message, value);
                return State.Success;
            }

            return State.Failure;
        }

        private Ray CalculateRay()
        {
            Ray ray = new Ray();

            if (origin.TryGetPosition3D(out Vector3 originValue))
            {
                ray.origin = originValue;
            }

            if (direction.TryGetPosition3D(out Vector3 directionValue))
            {
                ray.direction = (directionValue - originValue).normalized;
            }

            return ray;
        }
    }
}
