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
    [NodeContent("Line Cast", "Tasks/Physics/Line Cast", IconPath = "Images/Icons/Node/RayIcon.png")]
    public class LineCastTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key start;

        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key end;

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
            if (start == null || end == null)
            {
                return State.Failure;
            }

            Ray ray = CalculateRay();
            if (Physics.Linecast(ray.origin, ray.direction, out RaycastHit hitInfo, cullingLayer, queryTriggerInteraction))
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

            if (start.TryGetPosition3D(out Vector3 startValue))
            {
                ray.origin = startValue;
            }

            if (end.TryGetPosition3D(out Vector3 endValue))
            {
                ray.direction = endValue;
            }

            return ray;
        }
    }
}
