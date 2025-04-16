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
    [NodeContent("Line Cast 2D", "Tasks/Physics 2D/Line Cast 2D", IconPath = "Images/Icons/Node/RayIcon.png")]
    public class LineCast2DTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector2))]
        private Key start;

        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector2))]
        private Key end;

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
            if (start == null || end == null)
            {
                return State.Failure;
            }

            Ray2D ray = CalculateRay();
            RaycastHit2D hitInfo = Physics2D.Linecast(ray.origin, ray.direction, cullingLayer);
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

            if (start.TryGetPosition2D(out Vector2 startValue))
            {
                ray.origin = startValue;
            }

            if (end.TryGetPosition2D(out Vector2 endValue))
            {
                ray.direction = endValue;
            }

            return ray;
        }
    }
}
