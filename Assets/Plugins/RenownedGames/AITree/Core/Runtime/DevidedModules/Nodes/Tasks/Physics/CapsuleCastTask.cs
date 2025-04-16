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
    [NodeContent("Capsule Cast", "Tasks/Physics/Capsule Cast", IconPath = "Images/Icons/Node/RayIcon.png")]
    public class CapsuleCastTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private Vector3Key point1;

        [SerializeField]
        private Vector3Key point2;

        [SerializeField]
        private Vector3Key direction;

        [SerializeField]
        private float radius = 1;

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
            if (point1 == null || point2  == null || direction == null)
            {
                return State.Failure;
            }

            if (Physics.CapsuleCast(point1.GetValue(), point2.GetValue(), radius, direction.GetValue(), out RaycastHit hitInfo, maxDistance, cullingLayer, queryTriggerInteraction))
            {
                arg.TryCastValueTo<object>(out object value);
                hitInfo.transform.SendMessage(message, value);
                return State.Success;
            }

            return State.Failure;
        }
    }
}
